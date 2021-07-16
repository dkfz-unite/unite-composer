using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
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
        private readonly IIndexService<DonorIndex> _donorsIndexService;
        private readonly IIndexService<MutationIndex> _mutationsIndexService;

        public OncoGridDataService(IElasticOptions options)
        {
            _donorsIndexService = new DonorsIndexService(options);
            _mutationsIndexService = new MutationsIndexService(options);
        }

        public OncoGridData LoadData(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var donorsSearchResult = FindDonors(criteria);

            var donorIds = donorsSearchResult.Rows
                .Select(donor => donor.Id)
                .ToArray();

            var mutationsSearchResult = FindMutations(criteria, donorIds);

            var data = GetOncoGridData(
                donorsSearchResult.Rows,
                mutationsSearchResult.Rows,
                searchCriteria.OncoGridFilters.MostAffectedGeneCount
            );

            return data;
        }

        /// <summary>
        /// Retrieves donors with highest number of mutations filtered by given search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <returns>Search result with donors and number of total available rows.</returns>
        private SearchResult<DonorIndex> FindDonors(SearchCriteria searchCriteria)
        {
            var criteria = searchCriteria;

            var criteriaFilters = new DonorCriteriaFiltersCollection(criteria).All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(0, criteria.OncoGridFilters.MostAffectedDonorCount)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations)
                .AddExclusion(donor => donor.Mutations);
            // TODO: exclude all unnecessary information as soon as multiple exclusions work.
            // .AddExclusion(donor => donor.Treatments)
            // .AddExclusion(donor => donor.WorkPackages)
            // .AddExclusion(donor => donor.Studies);

            return _donorsIndexService.SearchAsync(query).Result;
        }

        /// <summary>
        /// Retrieves mutations of given donors in given genes filtered by given search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <param name="donorIds">Id's of donors</param>
        /// <returns>Search result with mutations and number of total available rows.</returns>
        private SearchResult<MutationIndex> FindMutations(SearchCriteria searchCriteria, IEnumerable<int> donorIds)
        {
            var criteria = new SearchCriteria();
            criteria.DonorFilters = new DonorCriteria();
            criteria.DonorFilters.Id = donorIds.ToArray();
            criteria.MutationFilters = searchCriteria.MutationFilters;

            var criteriaFilters = new MutationCriteriaFiltersCollection(criteria).All();

            var query = new SearchQuery<MutationIndex>()
                // TODO: remove magical number and include all possible mutations. This should be done properly with elasticsearch aggregations
                .AddPagination(0, 10000)
                .AddFilters(criteriaFilters)
                .AddFilter(new NotNullFilter<MutationIndex, object>("Mutation.HasAffectedTranscripts",
                    mutation => mutation.AffectedTranscripts))
                .AddExclusion(mutation => mutation.Donors.First().Specimens);
            // TODO: exclude all unnecessary information as soon as multiple exclusions work.
            // .AddExclusion(mutation => mutation.Donors.First().Studies)
            // .AddExclusion(mutation => mutation.Donors.First().Treatments)
            // .AddExclusion(mutation => mutation.Donors.First().ClinicalData)
            // .AddExclusion(mutation => mutation.Donors.First().WorkPackages);

            return _mutationsIndexService.SearchAsync(query).Result;
        }

        /// <summary>
        /// Builds <see cref="OncoGridData"/> object from donor and mutation indices for given number of most affected genes.
        /// </summary>
        /// <param name="donors">Donor indices</param>
        /// <param name="mutations">Mutation indices</param>
        /// <param name="numberOfGenes">Number of most affected genes</param>
        /// <returns><see cref="OncoGridData"/> object.</returns>
        private OncoGridData GetOncoGridData(
            IEnumerable<DonorIndex> donors,
            IEnumerable<MutationIndex> mutations,
            int numberOfGenes)
        {
            var oncoGridData = new OncoGridData();

            // Collections will be enumerated in controller, when building JSON object to return.
            // If immediate enumeration required, call 'ToArray' method for required data set.
            oncoGridData.Donors = GetDonorsData(donors);
            oncoGridData.Genes = GetGenesData(mutations, numberOfGenes);
            oncoGridData.Observations = GetObservationsData(oncoGridData.Donors, oncoGridData.Genes, mutations);

            return oncoGridData;
        }

        /// <summary>
        /// Build <see cref="OncoGridDonor"/> objects from donor indices.
        /// </summary>
        /// <param name="donors">Donor indices</param>
        /// <returns>Collection of <see cref="OncoGridDonor"/> objects.</returns>
        private IEnumerable<OncoGridDonor> GetDonorsData(IEnumerable<DonorIndex> donors)
        {
            return donors.Select(index => new OncoGridDonor(index));
        }

        /// <summary>
        /// Build <see cref="OncoGridGene"/> objects from mutation indices for given number of most affected genes.
        /// </summary>
        /// <param name="mutations">Mutation indices</param>
        /// <param name="numberOfGenes">Number of most affected genes</param>
        /// <returns>Collection of <see cref="OncoGridGene"/> objects.</returns>
        private IEnumerable<OncoGridGene> GetGenesData(IEnumerable<MutationIndex> mutations, int numberOfGenes)
        {
            return mutations
                .SelectMany(mutation => mutation.AffectedTranscripts)
                .Select(affectedTranscript => affectedTranscript.Gene)
                .GroupBy(gene => gene.Id)
                .OrderByDescending(group => group.Count())
                .Select(group => group.First())
                .Take(numberOfGenes)
                .Select(gene => new OncoGridGene(gene));
        }

        /// <summary>
        /// Builds <see cref="OncoGridMutation"/> objects from mutation indices
        /// for all combinations of given <see cref="OncoGridDonor"/> and <see cref="OncoGridGene"/> entries.
        /// </summary>
        /// <param name="donors">Donors to fill OncoGrid columns</param>
        /// <param name="genes">Genes to fill OncoGrid rows</param>
        /// <param name="mutations">Mutation indices</param>
        /// <returns>Collection of <see cref="OncoGridMutation"/> objects.</returns>
        private IEnumerable<OncoGridMutation> GetObservationsData(
            IEnumerable<OncoGridDonor> donors,
            IEnumerable<OncoGridGene> genes,
            IEnumerable<MutationIndex> mutations)
        {
            foreach (var donor in donors)
            {
                var donorId = int.Parse(donor.Id);

                foreach (var gene in genes)
                {
                    var geneId = int.Parse(gene.Id);

                    var observedMutations = mutations.Where(mutation =>
                        mutation.Donors.Any(mutationDonor => mutationDonor.Id == donorId) &&
                        mutation.AffectedTranscripts.Any(mutationTranscript => mutationTranscript.Gene.Id == geneId)
                    );

                    foreach (var mutation in observedMutations)
                    {
                        yield return new OncoGridMutation
                        {
                            Id = mutation.Id.ToString(),
                            Code = mutation.Code,
                            Type = mutation.Type,
                            Consequence = mutation.AffectedTranscripts
                                .Where(affectedTranscript => affectedTranscript.Gene.Id == geneId)
                                .SelectMany(affectedTranscript => affectedTranscript.Consequences)
                                .OrderBy(consequence => consequence.Severity)
                                .First().Type,
                            DonorId = donor.Id,
                            GeneId = gene.Id
                        };
                    }
                }
            }
        }
    }
}
