using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Data.Entities.Mutations;
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

        public OncoGridDataService(DonorsIndexService indexService)
        {
            _indexService = indexService;
        }

        public OncoGridData GetData(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new DonorCriteriaFiltersCollection(criteria)
                .All();

            var mostAffectedDonorCount = criteria.OncoGridFilters?.MostAffectedDonorCount ??
                                         new OncoGridCriteria().MostAffectedDonorCount;

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(0, mostAffectedDonorCount)
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
        /// 
        /// Important note:
        /// Each affectedTranscript can have multiple instances of the same gene.
        /// In order to not count them again as an occurrence the genes are grouped within the affectedTranscript.
        /// 
        /// </summary>
        /// <param name="mostAffectedDonorResources">top X donors of which the affected genes are queried from</param>
        /// <param name="mostAffectedGenes">limit of the resulting unique genes</param>
        /// <returns>A list of unique Genes for the oncogrid</returns>
        private List<OncoGridGeneData> CreateGenes(IEnumerable<DonorIndex> mostAffectedDonorResources,
            int mostAffectedGenes)
        {
            return mostAffectedDonorResources
                .SelectMany(donorIndex => donorIndex.Mutations
                    .Select(mutation => mutation.AffectedTranscripts?
                        //group by ensembleId in order to have only one GeneId in each mutation 
                        .GroupBy(transcript => transcript.Gene.EnsemblId,
                            at => at.Gene,
                            (key, group) => new {EnsembleId = key, Elements = group})))
                //Flatten the groups in order to calculate the mutations for each gene with another groupby
                .SelectMany(geneGroup => geneGroup)
                .GroupBy(geneGroup => geneGroup.EnsembleId,
                    geneGroup => geneGroup.Elements,
                    (key, group) => new {EnsembleId = key, Elements = group.First(), Count = group.Count()})
                .Select(geneGroup => new
                {
                    GeneResource = new OncoGridGeneData
                    {
                        Id = geneGroup.EnsembleId,
                        //Symbol of a gene can be null, if so we return the ensembleId
                        Symbol = geneGroup.Elements.FirstOrDefault()?.Symbol ?? geneGroup.EnsembleId
                    },
                    Count = geneGroup.Count
                })
                .OrderByDescending(group => group.Count)
                .Select(group => group.GeneResource)
                .Take(mostAffectedGenes)
                .ToList();
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
                        //Each Mutation affects multiple Transcripts. We select all the mutations which affects one of our mostAffectedGenes
                        .Where(affectedTranscript => mostAffectedGenes.Contains(affectedTranscript.Gene.EnsemblId))
                        //Map the affectedTranscript to a smaller object with just the highest consequence
                        .Select(affectedTranscript => new
                        {
                            Gene = affectedTranscript.Gene,
                            Consequence = affectedTranscript.Consequences.OrderBy(consequence => consequence.Severity)
                                .First()
                        })
                        //We want the most severe consequence first.
                        .OrderBy(affectedTranscript => affectedTranscript.Consequence.Severity)
                        //Group by each gene and select the most severe consequence
                        .GroupBy(
                            //Symbol can be missing so we group after ensembleid
                            affectedTranscript => affectedTranscript.Gene.EnsemblId,
                            affectedTranscript => affectedTranscript,
                            (key, group) => new {EnsembleId = key, Elements = group})
                        //Select only the most severe consequence for each gene
                        .Select(group => new ObservationData
                        {
                            GeneId = group.EnsembleId,
                            Consequence = group.Elements.First().Consequence.Type,
                            Type = mutation.Type,
                            DonorId = donorIndex.ReferenceId,
                            Id = mutation.Code
                        })
                    ));
        }
    }
}