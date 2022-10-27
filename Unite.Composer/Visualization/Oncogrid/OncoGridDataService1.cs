using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Visualization.Oncogrid;

public class OncoGridDataService1
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;


    public OncoGridDataService1(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
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
            searchCriteria.OncoGridFilters.NumberOfGenes
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
            .AddOrdering(donor => donor.NumberOfMutations)
            .AddExclusion(donor => donor.Specimens);
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
    private SearchResult<VariantIndex> FindMutations(
        SearchCriteria searchCriteria,
        IEnumerable<int> donorIds)
    {
        var criteria = new SearchCriteria();
        criteria.DonorFilters = new DonorCriteria();
        criteria.DonorFilters.Id = donorIds.ToArray();
        criteria.MutationFilters = searchCriteria.MutationFilters;

        var criteriaFilters = new MutationIndexFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            // TODO: remove magical number and include all possible mutations. This should be done properly with elasticsearch aggregations
            .AddPagination(0, 10000)
            .AddFilters(criteriaFilters)
            .AddFilter(new NotNullFilter<VariantIndex, object>("Variant.IsMutation", variant => variant.Mutation))
            .AddFilter(new NotNullFilter<VariantIndex, object>("Variant.HasAffectedFeatures", variant => variant.AffectedFeatures))
            .AddExclusion(variant => variant.Specimens.First().Donor.ClinicalData)
            .AddExclusion(variant => variant.Specimens.First().Donor.Treatments)
            .AddExclusion(variant => variant.Specimens.First().Donor.Studies)
            .AddExclusion(variant => variant.Specimens.First().Donor.Projects);
        // TODO: exclude all unnecessary information as soon as multiple exclusions work.
        // .AddExclusion(mutation => mutation.Donors.First().Studies)
        // .AddExclusion(mutation => mutation.Donors.First().Treatments)
        // .AddExclusion(mutation => mutation.Donors.First().ClinicalData)
        // .AddExclusion(mutation => mutation.Donors.First().WorkPackages);

        return _variantsIndexService.SearchAsync(query).Result;
    }


    /// <summary>
    /// Builds <see cref="OncoGridData"/> object from donor and variant indices for given number of most affected genes.
    /// </summary>
    /// <param name="donors">Donor indices.</param>
    /// <param name="variants">Variant indices.</param>
    /// <param name="numberOfGenes">Number of most affected genes.</param>
    /// <returns><see cref="OncoGridData"/> object.</returns>
    private OncoGridData GetOncoGridData(
        IEnumerable<DonorIndex> donors,
        IEnumerable<VariantIndex> variants,
        int numberOfGenes)
    {
        var oncoGridData = new OncoGridData();

        // Collections will be enumerated in controller, when building JSON object to return.
        // If immediate enumeration required, call 'ToArray' method for required data set.
        oncoGridData.Donors = GetDonorsData(donors);
        oncoGridData.Genes = GetGenesData(variants, numberOfGenes);
        oncoGridData.Observations = GetObservationsData(oncoGridData.Donors, oncoGridData.Genes, variants);

        return oncoGridData;
    }

    /// <summary>
    /// Build <see cref="OncoGridDonor"/> objects from donor indices.
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
    /// Build <see cref="OncoGridGene"/> objects from variant indices for given number of most affected genes.
    /// </summary>
    /// <param name="variants">Variant indices.</param>
    /// <param name="numberOfGenes">Number of most affected genes.</param>
    /// <returns>Collection of <see cref="OncoGridGene"/> objects.</returns>
    private IEnumerable<OncoGridGene> GetGenesData(
        IEnumerable<VariantIndex> variants, int numberOfGenes)
    {
        return variants
            .Where(variant => variant.Mutation != null && variant.AffectedFeatures != null)
            .SelectMany(variant => variant.AffectedFeatures)
            .Where(affectedFeature => affectedFeature.Transcript != null && affectedFeature.Gene != null)
            .Select(affectedFeature => affectedFeature.Gene)
            .GroupBy(gene => gene.Id)
            .OrderByDescending(group => group.Count())
            .Select(group => group.First())
            .Take(numberOfGenes)
            .Select(gene => new OncoGridGene(gene));
    }

    /// <summary>
    /// Builds <see cref="OncoGridMutation"/> objects from variant indices
    /// for all combinations of given <see cref="OncoGridDonor"/> and <see cref="OncoGridGene"/> entries.
    /// </summary>
    /// <param name="donors">Donors to fill OncoGrid columns.</param>
    /// <param name="genes">Genes to fill OncoGrid rows.</param>
    /// <param name="variants">Variant indices.</param>
    /// <returns>Collection of <see cref="OncoGridMutation"/> objects.</returns>
    private IEnumerable<OncoGridMutation> GetObservationsData(
        IEnumerable<OncoGridDonor> donors,
        IEnumerable<OncoGridGene> genes,
        IEnumerable<VariantIndex> variants)
    {
        foreach (var donor in donors)
        {
            var donorId = int.Parse(donor.Id);

            foreach (var gene in genes)
            {
                var geneId = int.Parse(gene.Id);

                var observedVariants = variants.Where(variant =>
                    variant.Mutation != null &&
                    variant.Specimens.Any(specimen => specimen.Donor.Id == donorId) &&
                    variant.AffectedFeatures.Any(affectedFeature =>
                        affectedFeature.Transcript != null &&
                        affectedFeature.Gene != null &&
                        affectedFeature.Gene.Id == geneId)
                );

                foreach (var variant in observedVariants)
                {
                    var consequence = variant.AffectedFeatures
                        .Where(affectedFeature => affectedFeature.Transcript != null)
                        .Where(affectedFeature => affectedFeature.Gene != null)
                        .Where(affectedFeature => affectedFeature.Gene.Id == geneId)
                        .SelectMany(affectedFeature => affectedFeature.Consequences)
                        .OrderBy(consequence => consequence.Severity)
                        .First();

                    yield return new OncoGridMutation
                    {
                        Id = variant.Id,
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
