using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Visualization.Oncogrid;

public class OncoGridDataService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;


    public OncoGridDataService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public OncoGridData LoadData(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var impacts = criteria.MutationFilters.Impact;
        var consequences = criteria.MutationFilters.Consequence;

        var donorsSearchResult = FindDonors(criteria);

        var donorIds = donorsSearchResult.Rows
            .Select(donor => donor.Id)
            .ToArray();

        var genesSearchResult = FindGenes(criteria, donorIds);

        var geneIds = genesSearchResult.Rows
            .Select(gene => gene.Id)
            .ToArray();

        var mutationsSearchResult = FindMutations(criteria, donorIds, geneIds);

        var data = GetOncoGridData(
            donorsSearchResult.Rows,
            genesSearchResult.Rows,
            mutationsSearchResult.Rows,
            impacts, consequences
        );

        return data;
    }


    /// <summary>
    /// Retrieves donors with highest number of mutations filtered by given search criteria.
    /// </summary>
    /// <param name="searchCriteria">Search criteria</param>
    /// <returns>Search result with donors and number of total available rows.</returns>
    private SearchResult<DonorIndex> FindDonors(
        SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var criteriaFilters = new DonorIndexFiltersCollection(criteria).All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(0, criteria.OncoGridFilters.NumberOfDonors)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(donor => donor.NumberOfGenes)
            .AddExclusion(donor => donor.Specimens)
            .AddExclusion(donor => donor.Treatments)
            .AddExclusion(donor => donor.Projects)
            .AddExclusion(donor => donor.Studies);

        return _donorsIndexService.SearchAsync(query).Result;
    }

    /// <summary>
    /// Retrieves genes of given donors with highest number of mutations filtered by given search criteria.
    /// </summary>
    /// <param name="searchCriteria">Search criteria</param>
    /// <param name="donorIds">Id's of donors</param>
    /// <returns>Search result with genes and number of total available rows.</returns>
    private SearchResult<GeneIndex> FindGenes(
        SearchCriteria searchCriteria,
        IEnumerable<int> donorIds)
    {
        var criteria = new SearchCriteria();
        criteria.DonorFilters = new DonorCriteria();
        criteria.DonorFilters.Id = donorIds.ToArray();
        criteria.GeneFilters = searchCriteria.GeneFilters;
        criteria.MutationFilters = searchCriteria.MutationFilters;
        criteria.OncoGridFilters = searchCriteria.OncoGridFilters;

        var criteriaFilters = new GeneIndexFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, criteria.OncoGridFilters.NumberOfGenes)
            .AddFilters(criteriaFilters)
            .AddOrdering(gene => gene.NumberOfMutations)
            .AddExclusion(gene => gene.Specimens);

        return _genesIndexService.SearchAsync(query).Result;
    }

    /// <summary>
    /// Retrieves mutations of given donors in given genes filtered by given search criteria.
    /// </summary>
    /// <param name="searchCriteria">Search criteria</param>
    /// <param name="donorIds">Id's of donors</param>
    /// <param name="geneIds">Id's of genes</param>
    /// <returns>Search result with mutations and number of total available rows.</returns>
    private SearchResult<VariantIndex> FindMutations(
        SearchCriteria searchCriteria,
        IEnumerable<int> donorIds,
        IEnumerable<int> geneIds)
    {
        var criteria = new SearchCriteria();
        criteria.DonorFilters = new DonorCriteria();
        criteria.DonorFilters.Id = donorIds.ToArray();
        criteria.GeneFilters = new GeneCriteria();
        criteria.GeneFilters.Id = geneIds.ToArray();
        criteria.MutationFilters = searchCriteria.MutationFilters;
        criteria.OncoGridFilters = searchCriteria.OncoGridFilters;

        var criteriaFilters = new MutationIndexFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            // TODO: remove magical number and include all possible mutations. This should be done properly with elasticsearch aggregations
            .AddPagination(0, 10000)
            .AddFilters(criteriaFilters)
            .AddFilter(new NotNullFilter<VariantIndex, object>("Variant.IsMutation", variant => variant.Mutation))
            .AddFilter(new NotNullFilter<VariantIndex, object>("Variant.HasAffectedFeatures", variant => variant.Mutation.AffectedFeatures))
            .AddExclusion(mutation => mutation.Specimens.First().Donor.ClinicalData)
            .AddExclusion(mutation => mutation.Specimens.First().Donor.Treatments)
            .AddExclusion(mutation => mutation.Specimens.First().Donor.Projects)
            .AddExclusion(mutation => mutation.Specimens.First().Donor.Studies);

        return _variantsIndexService.SearchAsync(query).Result;
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
        IEnumerable<GeneIndex> genes,
        IEnumerable<VariantIndex> mutations,
        IEnumerable<string> impacts,
        IEnumerable<string> consequences)
    {
        var oncoGridData = new OncoGridData();

        // Collections will be enumerated in controller, when building JSON object to return.
        // If immediate enumeration required, call 'ToArray' method for required data set.
        oncoGridData.Donors = GetDonorsData(donors);
        oncoGridData.Genes = GetGenesData(genes);
        oncoGridData.Observations = GetObservationsData(oncoGridData.Donors, oncoGridData.Genes, mutations, impacts, consequences);

        return oncoGridData;
    }

    /// <summary>
    /// Builds <see cref="OncoGridDonor"/> objects from donor indices.
    /// </summary>
    /// <param name="donors">Donor indices</param>
    /// <returns>Collection of <see cref="OncoGridDonor"/> objects.</returns>
    private IEnumerable<OncoGridDonor> GetDonorsData(
        IEnumerable<DonorIndex> donors)
    {
        return donors
            .Select(index => new OncoGridDonor(index));
    }

    /// <summary>
    /// Builds <see cref="OncoGridGene"/> objects from gene indices.
    /// </summary>
    /// <param name="genes">Gene indices</param>
    /// <returns>Collection of <see cref="OncoGridGene"/> objects.</returns>
    private IEnumerable<OncoGridGene> GetGenesData(
        IEnumerable<GeneIndex> genes)
    {
        return genes
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
        IEnumerable<VariantIndex> mutations,
        IEnumerable<string> impacts,
        IEnumerable<string> consequences)
    {
        foreach (var donor in donors)
        {
            var donorId = int.Parse(donor.Id);

            foreach (var gene in genes)
            {
                var geneId = int.Parse(gene.Id);

                var observedMutations = mutations.Where(mutation =>
                    mutation.Specimens.Any(mutationSpecimen => mutationSpecimen.Donor.Id == donorId) &&
                    mutation.Mutation.AffectedFeatures.Any(affectedFeature =>
                        affectedFeature.Transcript != null &&
                        affectedFeature.Gene != null &&
                        affectedFeature.Gene.Id == geneId)
                );

                foreach (var mutation in observedMutations)
                {
                    var consequence = mutation.Mutation.AffectedFeatures
                        .Where(affectedFeature => affectedFeature.Transcript != null)
                        .Where(affectedFeature => affectedFeature.Gene != null)
                        .Where(affectedFeature => affectedFeature.Gene.Id == geneId)
                        .SelectMany(affectedFeature => affectedFeature.Consequences)
                        .Where(consequence => HasMatchingImpact(consequence.Impact, impacts))
                        .Where(consequence => HasMatchingConsequence(consequence.Type, consequences))
                        .OrderBy(consequence => consequence.Severity)
                        .FirstOrDefault();

                    if (consequence != null)
                    {
                        yield return new OncoGridMutation
                        {
                            Id = mutation.Id,
                            Code = GetVariantCode(mutation),
                            Consequence = consequence.Type,
                            Impact = consequence.Impact,
                            DonorId = donor.Id,
                            GeneId = gene.Id
                        };
                    }
                }
            }
        }
    }


    private static bool HasMatchingImpact(string impact, IEnumerable<string> impacts)
    {
        return impacts == null || !impacts.Any() || impacts.Contains(impact);
    }

    private static bool HasMatchingConsequence(string consequence, IEnumerable<string> consequences)
    {
        return consequences == null || !consequences.Any() || consequences.Contains(consequence);
    }

    private string GetVariantCode(VariantIndex variant)
    {
        return $"{variant.Mutation.Chromosome}:g.{variant.Mutation.Start}{variant.Mutation.Ref ?? "-"}>{variant.Mutation.Alt ?? "-"}";
    }
}
