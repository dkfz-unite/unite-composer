using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Indices.Services.Configuration.Options;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Visualization.Oncogrid
{
    public class OncoGridDataService
    {
        private readonly IIndexService<DonorIndex> _donorService;
        private readonly IIndexService<MutationIndex> _mutationService;

        public OncoGridDataService(IElasticOptions options)
        {
            _donorService = new DonorsIndexService(options);
            _mutationService = new MutationsIndexService(options);
        }

        public OncoGridDataService(DonorsIndexService donorService)
        {
            _donorService = donorService;
        }

        public OncoGridData GetData(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();
            var donorResult = FindMostAffectedDonors(criteria);
            var mutationResult = FindMutationsForDonors(criteria, donorResult);

            return From(mutationResult.Rows, donorResult.Rows, searchCriteria);
        }

        private SearchResult<MutationIndex> FindMutationsForDonors(SearchCriteria criteria,
            SearchResult<DonorIndex> donorResult)
        {
            // Filter only the mutations which belongs to the most affected donors
            criteria.DonorFilters = new DonorCriteria
            {
                ReferenceId = donorResult.Rows.Select(donor => donor.ReferenceId).ToArray()
            };

            criteria.TissueFilters = null;
            criteria.CellLineFilters = null;

            var mutationCriteriaFilters = new MutationCriteriaFiltersCollection(criteria).All();

            var mutationQuery = new SearchQuery<MutationIndex>()
                //TODO: remove magical number and include all possible mutations. This should be done properly with elasticsearch aggregations
                .AddPagination(0, 10000)
                .AddFilters(mutationCriteriaFilters)
                .AddExclusion(mutation => mutation.Donors.First().Specimens);
            //TODO: exclude all unnecessary information as soon as multiple exclusions work.
            // .AddExclusion(mutation => mutation.Donors.First().Studies)
            // .AddExclusion(mutation => mutation.Donors.First().Treatments)
            // .AddExclusion(mutation => mutation.Donors.First().ClinicalData)
            // .AddExclusion(mutation => mutation.Donors.First().WorkPackages);

            return _mutationService.SearchAsync(mutationQuery).Result;
        }

        private SearchResult<DonorIndex> FindMostAffectedDonors(SearchCriteria criteria)
        {
            var donorCriteriaFilters = new DonorCriteriaFiltersCollection(criteria).All();

            var mostAffectedDonorCount = criteria.OncoGridFilters?.MostAffectedDonorCount ??
                                         new OncoGridCriteria().MostAffectedDonorCount;

            var donorQuery = new SearchQuery<DonorIndex>()
                .AddPagination(0, mostAffectedDonorCount)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(donorCriteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations)
                .AddExclusion(donor => donor.Mutations);
            //TODO: exclude all unnecessary information as soon as multiple exclusions work.
            // .AddExclusion(donor => donor.Treatments)
            // .AddExclusion(donor => donor.WorkPackages)
            // .AddExclusion(donor => donor.Studies);

            return _donorService.SearchAsync(donorQuery).Result;
        }

        private OncoGridData From(IEnumerable<MutationIndex> mutations,
            IEnumerable<DonorIndex> mostAffectedDonors, SearchCriteria searchCriteria)
        {
            var oncoGridFilter = searchCriteria?.OncoGridFilters;
            var mostAffectedGeneCount = oncoGridFilter?.MostAffectedGeneCount ?? 50;

            var oncoGridDonorResources = mostAffectedDonors.Select(index => new OncoGridDonorData(index));

            var mostAffectedGeneResources = CreateGenes(mutations, mostAffectedGeneCount);

            var distinctEnsembleIds = mostAffectedGeneResources.Select(res => res.Id);
            var observationResources = CreateObservations(mostAffectedDonors, mutations, distinctEnsembleIds);
            var uniqueDonorsForOncoGrid = observationResources.Select(res => res.DonorId).Distinct();

            var oncoGridResource = new OncoGridData();
            oncoGridResource.Donors.AddRange(
                oncoGridDonorResources.Where(donor => uniqueDonorsForOncoGrid.Contains(donor.Id)));
            oncoGridResource.Genes.AddRange(mostAffectedGeneResources);
            oncoGridResource.Observations.AddRange(observationResources);
            return oncoGridResource;
        }

        /// <summary>
        /// Selects the top Genes ordered by the occurrence.
        /// 
        /// Mutations.AffectedTranscripts are flattened and grouped by the ensembleID and gene-symbol of the transcript-gene.
        /// This genes are then ordered by the count, which represents the actual occurence within mutations.
        /// </summary>
        /// <param name="mutations"></param>
        /// <param name="mostAffectedGenes">limit of the resulting unique genes</param>
        /// <returns>A list of unique Genes for the oncogrid</returns>
        private List<OncoGridGeneData> CreateGenes(IEnumerable<MutationIndex> mutations,
            int mostAffectedGenes)
        {
            // create a mapping of each gene to the actual occurence amount within all transcripts 
            return mutations
                .SelectMany(mutation => mutation.AffectedTranscripts)
                .GroupBy(transcript => (transcript.Gene.EnsemblId, transcript.Gene.Symbol))
                .OrderByDescending(group => group.Count())
                .Take(mostAffectedGenes)
                .Select(group => new OncoGridGeneData
                {
                    Id = group.Key.EnsemblId,
                    //Symbol of a gene can be null, if so we return the ensembleId
                    Symbol = group.Key.Symbol ?? group.Key.EnsemblId
                })
                .ToList();
        }

        /// <summary>
        /// Creates a list of <see cref="ObservationData"/> which represents an oncogrid column entry.
        ///
        /// Based on Donors the actual Mutations.AffectedTranscripts are flattened and filtered by the ensembleid within mostAffectedGenes.
        /// </summary>
        /// <returns>A list of column entries for the oncogrid</returns>
        private IEnumerable<ObservationData> CreateObservations(
            IEnumerable<DonorIndex> donors,
            IEnumerable<MutationIndex> mutations,
            IEnumerable<string> mostAffectedGenes)
        {
            var observationDatas = new List<ObservationData>();

            foreach (var donor in donors)
            {
                var observationDataForDonorMutations =
                    CreateObservationDataForDonorMutations(mutations, mostAffectedGenes, donor);
                observationDatas.AddRange(observationDataForDonorMutations);
            }

            return observationDatas;
        }

        private static ObservationData[] CreateObservationDataForDonorMutations(
            IEnumerable<MutationIndex> mutations,
            IEnumerable<string> mostAffectedGenes, DonorIndex donor)
        {
            var observationDataForDonorMutations = mutations
                //Filter the mutation according to the donorId in order to create an entry for each donor and gene combination available.
                .Where(mutation => mutation.Donors.Select(donor => donor.ReferenceId).Contains(donor.ReferenceId))
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
                        (key, group) => new {EnsembleId = key, Elements = @group})
                    //Select only the most severe consequence for each gene
                    .Select(group => new ObservationData
                    {
                        GeneId = group.EnsembleId,
                        Consequence = group.Elements.First().Consequence.Type,
                        Type = mutation.Type,
                        DonorId = donor.ReferenceId,
                        Id = mutation.Code
                    })
                ).ToArray();
            return observationDataForDonorMutations;
        }
    }
}