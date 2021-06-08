using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Visualization.Oncogrid
{
    public class OncoGridDataService
    {
        private readonly IIndexService<DonorIndex> _indexService;

        public OncoGridDataService(IElasticOptions options)
        {
            _indexService = new DonorsIndexService(options);
        }

        //TODO: Create oncogrid search criteria similar to search criteria if required
        public OncoGridData GetData(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new DonorCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(0, criteria.OncoGridFilters.MostAffectedDonorCount)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations);

            var result = _indexService.SearchAsync(query).Result;

            return From(result.Rows, searchCriteria?.OncoGridFilters);
        }


        private OncoGridData From(IEnumerable<DonorIndex> donors, OncoGridCriteria filter)
        {
            var mostAffectedDonorCount = filter?.MostAffectedDonorCount ?? 200;
            var mostAffectedGeneCount = filter?.MostAffectedGeneCount ?? 50;

            var mostAffectedDonors = GetMostAffectedDonors(donors, mostAffectedDonorCount);
            var oncoGridDonorResources = mostAffectedDonors.Select(index => new OncoGridDonorData(index));
            var mostAffectedGeneResources = CreateGenes(mostAffectedDonors, mostAffectedGeneCount);
            var distinctEnsembleIds = mostAffectedGeneResources.Select(res => res.Id);
            var observationResources = CreateObservations(mostAffectedDonors, distinctEnsembleIds);

            var oncoGridResource = new OncoGridData();
            oncoGridResource.Donors.AddRange(oncoGridDonorResources);
            oncoGridResource.Genes.AddRange(mostAffectedGeneResources);
            oncoGridResource.Observations.AddRange(observationResources);
            return oncoGridResource;
        }


        /// <summary>
        /// Selects the top <see cref="OncoGridCriteria.MostAffectedDonorCount"/> Donors ordered by <see cref="DonorIndex.NumberOfMutations"/>
        /// </summary>
        /// <param name="donors">List of Donors which should be limited</param>
        /// <param name="mostAffectedDonorCount">the actual limit of the donors</param>
        /// <returns>A list of unique Donors for the OncoGrid</returns>
        private static List<DonorIndex> GetMostAffectedDonors(IEnumerable<DonorIndex> donors,
            int mostAffectedDonorCount)
        {
            return donors.OrderBy(resource => resource.NumberOfMutations)
                .Take(mostAffectedDonorCount)
                .ToList();
        }

        /// <summary>
        /// Selects the top Genes ordered by the occurrence.
        /// 
        /// Based on Donors the actual Mutations.AffectedTranscripts are flattened and grouped by the ensembleID of the transcript-gene.
        /// This genes are then ordered by the count, which represents the actual occurence within mutations.
        /// </summary>
        /// <param name="mostAffectedDonorResources">top X donors of which the affected genes are queried from</param>
        /// <param name="mostAffectedGenes">limit of the resulting unique genes</param>
        /// <returns>A list of unique Genes for the oncogrid</returns>
        private List<OncoGridGeneData> CreateGenes(IEnumerable<DonorIndex> mostAffectedDonorResources,
            int mostAffectedGenes)
        {
            return mostAffectedDonorResources
                .SelectMany(donorIndex => donorIndex.Mutations
                    .SelectMany(mutation => mutation.AffectedTranscripts?
                        .GroupBy(transcript => new {transcript.Gene.EnsemblId, transcript.Gene.Symbol})
                        .Select(geneGroup => new
                        {
                            GeneResource = new OncoGridGeneData
                            {
                                Id = geneGroup.Key.EnsemblId,
                                Symbol = geneGroup.Key.Symbol
                            },
                            Count = geneGroup.Count()
                        })
                    ))
                .OrderByDescending(group => group.Count)
                .Select(group => group.GeneResource)
                // TODO: Distinct shouldnt be required because the list is grouped... remove the comparer
                // .Distinct(OncoGridGeneData.EnsemblIdComparer)
                .Take(mostAffectedGenes)
                .ToList();
            ;
        }

        /// <summary>
        /// Creates a list of <see cref="ObservationData"/> which represents an oncogrid column entry.
        ///
        /// Based on Donors the actual Mutations.AffectedTranscripts are flattened and filtered by the ensembleid within mostAffectedGenes.
        /// </summary>
        /// <param name="mostAffectedDonorResources">top X donors of which the mutations are prepared for the oncogrid</param>
        /// <param name="mostAffectedGenes">A list of <see cref="GeneIndex.EnsemblId"/> which is used to filter the mutation-transcripts</param>
        /// <returns>A list of column entries for the oncogrid</returns>
        private IEnumerable<ObservationData> CreateObservations(IEnumerable<DonorIndex> mostAffectedDonorResources,
            IEnumerable<string> mostAffectedGenes)
        {
            return mostAffectedDonorResources
                .SelectMany(donorIndex => donorIndex.Mutations
                    .SelectMany(mutation => mutation.AffectedTranscripts?
                        .Where(transcript => mostAffectedGenes.Contains(transcript.Gene.EnsemblId))
                        .SelectMany(transcript => transcript.Consequences
                            .Select(consequence => new ObservationData
                            {
                                Type = mutation.Type,
                                DonorId = donorIndex.ReferenceId,
                                GeneId = transcript.Gene.EnsemblId,
                                Consequence = consequence.Type,
                                Id = mutation.Code
                            }))));
        }
    }
}