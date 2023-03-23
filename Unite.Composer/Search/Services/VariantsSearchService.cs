using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class VariantsSearchService : IVariantsSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;


    public VariantsSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public VariantIndex Get(string key, VariantSearchContext searchContext = null)
    {
        var query = new GetQuery<VariantIndex>(key)
            .AddExclusion(variant => variant.Samples);

        var result = _variantsIndexService.GetAsync(query).Result;

        return result;
    }

    public SearchResult<VariantIndex> Search(SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new VariantSearchContext();

        var criteriaFilters = GetFiltersCollection(criteria, context)
            .All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(mutation => mutation.NumberOfDonors)
            .AddExclusion(mutation => mutation.Samples);

        var result = _variantsIndexService.SearchAsync(query).Result;

        return result;
    }

    public SearchResult<DonorIndex> SearchDonors(string variantId, SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.MutationFilters = new MutationCriteria { Id = new[] { variantId } };

        var criteriaFilters = new DonorIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(donor => donor.NumberOfMutations)
            .AddExclusion(donor => donor.Specimens);

        var result = _donorsIndexService.SearchAsync(query).Result;

        return result;
    }


    private FiltersCollection<VariantIndex> GetFiltersCollection(SearchCriteria criteria, VariantSearchContext context)
    {
        return context.VariantType switch
        {
            VariantType.SSM => new MutationIndexFiltersCollection(criteria),
            VariantType.CNV => new CopyNumberVariantFiltersCollection(criteria),
            VariantType.SV => new StructuralVariantFiltersCollection(criteria),
            _ => new VariantFiltersCollection(criteria)
        };
    }
}
