using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.SsmIndex;

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
        _variantsIndexService = new SsmsIndexService(options);
    }


    public OncoGridData LoadData(int numberOfDonors = 100, int numberOfGenes = 50, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var impacts = criteria.Ssm.Impact;
        var effect = criteria.Ssm.Effect;

        var donorsSearchResult = FindDonors(numberOfDonors, criteria);

        var donorIds = donorsSearchResult.Rows
            .Select(donor => donor.Id)
            .ToArray();

        var genesSearchResult = FindGenes(numberOfGenes, criteria, donorIds);

        var geneIds = genesSearchResult.Rows
            .Select(gene => gene.Id)
            .ToArray();

        var mutationsSearchResult = FindMutations(criteria, donorIds, geneIds);

        var data = GetOncoGridData(
            donorsSearchResult.Rows,
            genesSearchResult.Rows,
            mutationsSearchResult.Rows,
            impacts, effect
        );

        return data;
    }


    /// <summary>
    /// Retrieves donors with highest number of mutations filtered by given search criteria.
    /// </summary>
    /// <param name="number">Number of donors to retrieve.</param> 
    /// <param name="searchCriteria">Search criteria.</param>
    /// <returns>Search result with donors and number of total available rows.</returns>
    private SearchResult<DonorIndex> FindDonors(
        int number,
        SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new DonorFiltersCollection(criteria).All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(0, number)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(donor => donor.Stats.Genes)
            .AddExclusion(donor => donor.Specimens)
            .AddExclusion(donor => donor.Treatments)
            .AddExclusion(donor => donor.Projects)
            .AddExclusion(donor => donor.Studies);

        return _donorsIndexService.Search(query).Result;
    }

    /// <summary>
    /// Retrieves genes of given donors with highest number of mutations filtered by given search criteria.
    /// </summary>
    /// <param name="number">Number of genes to retrieve.</param> 
    /// <param name="searchCriteria">Search criteria.</param>
    /// <param name="donorIds">Id's of donors.</param>
    /// <returns>Search result with genes and number of total available rows.</returns>
    private SearchResult<GeneIndex> FindGenes(
        int number,
        SearchCriteria searchCriteria,
        IEnumerable<int> donorIds)
    {
        var criteria = new SearchCriteria();
        criteria.Donor = new DonorCriteria() { Id = donorIds.ToArray() };
        criteria.Gene = searchCriteria.Gene;
        criteria.Ssm = searchCriteria.Ssm;

        var criteriaFilters = new GeneFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, number)
            .AddFilters(criteriaFilters)
            .AddOrdering(gene => gene.Stats.Ssms)
            .AddExclusion(gene => gene.Specimens);

        return _genesIndexService.Search(query).Result;
    }

    /// <summary>
    /// Retrieves mutations of given donors in given genes filtered by given search criteria.
    /// </summary>
    /// <param name="searchCriteria">Search criteria.</param>
    /// <param name="donorIds">Id's of donors.</param>
    /// <param name="geneIds">Id's of genes.</param>
    /// <returns>Search result with mutations and number of total available rows.</returns>
    private SearchResult<VariantIndex> FindMutations(
        SearchCriteria searchCriteria,
        IEnumerable<int> donorIds,
        IEnumerable<int> geneIds)
    {
        var criteria = new SearchCriteria();
        criteria.Donor = new DonorCriteria() { Id = donorIds.ToArray() };
        criteria.Gene = new GeneCriteria() { Id = geneIds.ToArray() };
        criteria.Ssm = searchCriteria.Ssm;

        var criteriaFilters = new SsmFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 10000)
            .AddFilters(criteriaFilters)
            .AddFilter(new NotNullFilter<VariantIndex, object>("SSM.HasAffectedFeatures", variant => variant.AffectedFeatures));

        return _variantsIndexService.Search(query).Result;
    }


    /// <summary>
    /// Builds <see cref="OncoGridData"/> object from donor and mutation indices for given number of most affected genes.
    /// </summary>
    /// <param name="donors">Donor indices.</param>
    /// <param name="mutations">Mutation indices.</param>
    /// <param name="numberOfGenes">Number of most affected genes.</param>
    /// <returns><see cref="OncoGridData"/> object.</returns>
    private static OncoGridData GetOncoGridData(
        IEnumerable<DonorIndex> donors,
        IEnumerable<GeneIndex> genes,
        IEnumerable<VariantIndex> mutations,
        IEnumerable<string> impacts,
        IEnumerable<string> effects)
    {
        var oncoGridData = new OncoGridData();

        // Collections will be enumerated in controller, when building JSON object to return.
        // If immediate enumeration required, call 'ToArray' method for required data set.
        oncoGridData.Donors = GetDonorsData(donors);
        oncoGridData.Genes = GetGenesData(genes);
        oncoGridData.Observations = GetObservationsData(oncoGridData.Donors, oncoGridData.Genes, mutations, impacts, effects);

        return oncoGridData;
    }

    /// <summary>
    /// Builds <see cref="OncoGridDonor"/> objects from donor indices.
    /// </summary>
    /// <param name="donors">Donor indices.</param>
    /// <returns>Collection of <see cref="OncoGridDonor"/> objects.</returns>
    private static IEnumerable<OncoGridDonor> GetDonorsData(
        IEnumerable<DonorIndex> donors)
    {
        return donors
            .Select(index => new OncoGridDonor(index));
    }

    /// <summary>
    /// Builds <see cref="OncoGridGene"/> objects from gene indices.
    /// </summary>
    /// <param name="genes">Gene indices.</param>
    /// <returns>Collection of <see cref="OncoGridGene"/> objects.</returns>
    private static IEnumerable<OncoGridGene> GetGenesData(
        IEnumerable<GeneIndex> genes)
    {
        return genes
            .Select(gene => new OncoGridGene(gene));
    }

    /// <summary>
    /// Builds <see cref="OncoGridVariant"/> objects from mutation indices
    /// for all combinations of given <see cref="OncoGridDonor"/> and <see cref="OncoGridGene"/> entries.
    /// </summary>
    /// <param name="donors">Donors to fill OncoGrid columns.</param>
    /// <param name="genes">Genes to fill OncoGrid rows.</param>
    /// <param name="mutations">Mutation indices.</param>
    /// <returns>Collection of <see cref="OncoGridVariant"/> objects.</returns>
    private static IEnumerable<OncoGridVariant> GetObservationsData(
        IEnumerable<OncoGridDonor> donors,
        IEnumerable<OncoGridGene> genes,
        IEnumerable<VariantIndex> mutations,
        IEnumerable<string> impacts,
        IEnumerable<string> effects)
    {
        foreach (var donor in donors)
        {
            var donorId = int.Parse(donor.Id);

            foreach (var gene in genes)
            {
                var geneId = int.Parse(gene.Id);

                var observedMutations = mutations.Where(mutation =>
                    // mutation.Specimens.Any(mutationSample => mutationSample.Donor.Id == donorId) &&
                    mutation.AffectedFeatures.Any(affectedFeature =>
                        affectedFeature.Transcript != null &&
                        affectedFeature.Gene != null &&
                        affectedFeature.Gene.Id == geneId)
                );

                foreach (var mutation in observedMutations)
                {
                    var effect = mutation.AffectedFeatures
                        .Where(affectedFeature => affectedFeature.Transcript != null)
                        .Where(affectedFeature => affectedFeature.Gene != null)
                        .Where(affectedFeature => affectedFeature.Gene.Id == geneId)
                        .SelectMany(affectedFeature => affectedFeature.Effects)
                        .Where(effect => HasMatchingImpact(effect.Impact, impacts))
                        .Where(effect => HasMatchingEffects(effect.Type, effects))
                        .OrderBy(effect => effect.Severity)
                        .FirstOrDefault();

                    if (effect != null)
                    {
                        yield return new OncoGridVariant
                        {
                            Id = $"{mutation.Id}",
                            Code = GetVariantCode(mutation),
                            Effect = effect.Type,
                            Impact = effect.Impact,
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

    private static bool HasMatchingEffects(string effect, IEnumerable<string> effects)
    {
        return effects == null || !effects.Any() || effects.Contains(effect);
    }

    private static string GetVariantCode(VariantIndex variant)
    {
        return $"{variant.Chromosome}:g.{variant.Start}{variant.Ref ?? "-"}>{variant.Alt ?? "-"}";
    }
}
