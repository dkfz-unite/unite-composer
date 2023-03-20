using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class GenesSearchService : IGenesSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public GenesSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }

    public GeneIndex Get(string key)
    {
        var query = new GetQuery<GeneIndex>(key)
            .AddExclusion(gene => gene.Specimens);

        var result = _genesIndexService.GetAsync(query).Result;

        return result;
    }

    public SearchResult<GeneIndex> Search(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var criteriaFilters = new GeneIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(gene => gene.NumberOfDonors)
            .AddExclusion(gene => gene.Specimens);

        var result = _genesIndexService.SearchAsync(query).Result;

        return result;
    }

    public SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var criteriaFilters = new DonorIndexFiltersCollection(criteria)
            .All();

        var geneFilter = new MultyPropertyEqualityFilter<DonorIndex, int>(
            "Gene.Id",
            donor => donor.Specimens.First().Variants.First().Mutation.AffectedFeatures.First().Gene.Id,
            donor => donor.Specimens.First().Variants.First().CopyNumberVariant.AffectedFeatures.First().Gene.Id,
            donor => donor.Specimens.First().Variants.First().StructuralVariant.AffectedFeatures.First().Gene.Id,
            geneId
        );

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFilters(criteriaFilters)
            .AddFilter(geneFilter)
            .AddOrdering(donor => donor.NumberOfMutations);

        var result = _donorsIndexService.SearchAsync(query).Result;

        return result;
    }

    public SearchResult<VariantIndex> SearchVariants(int geneId, VariantType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.GeneFilters = new GeneCriteria { Id = new[] { geneId } };

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(mutation => mutation.NumberOfDonors)
            .AddExclusion(mutation => mutation.Specimens);

        var result = _variantsIndexService.SearchAsync(query).Result;

        return result;
    }


    private FiltersCollection<VariantIndex> GetFiltersCollection(VariantType type, SearchCriteria criteria)
    {
        return type switch
        {
            VariantType.SSM => new MutationIndexFiltersCollection(criteria),
            VariantType.CNV => new CopyNumberVariantFiltersCollection(criteria),
            VariantType.SV => new StructuralVariantFiltersCollection(criteria),
            _ => new VariantFiltersCollection(criteria)
        };
    }
}
