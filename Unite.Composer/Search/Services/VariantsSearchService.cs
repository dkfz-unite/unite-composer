using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class VariantsSearchService : AggregatingSearchService, IVariantsSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;


    public VariantsSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }

    
    public VariantIndex Get(string key, VariantSearchContext searchContext = null)
    {
        var query = new GetQuery<VariantIndex>(key)
            .AddExclusion(variant => variant.Samples);

        var result = _variantsIndexService.GetAsync(query).Result;

        return result;
    }


    public IDictionary<long, DataIndex> Stats(SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        var context = searchContext ?? new VariantSearchContext();

        var availableData = new Dictionary<long, DataIndex>();

        criteria = criteria with { From = 0, Size = 0 };
        var lookupResult = Search(criteria, context);

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };
            var searchResult = Search(criteria, context);

            foreach (var index in searchResult.Rows)
            {
                // Bad code, add real Id to index
                var id = index.Id.StartsWith("SSM") ? long.Parse(index.Id.Substring(3))
                       : index.Id.StartsWith("CNV") ? long.Parse(index.Id.Substring(3))
                       : index.Id.StartsWith("SV") ? long.Parse(index.Id.Substring(2))
                       : throw new InvalidOperationException($"Unknown variant type: {index.Id}");

                availableData.Add(id, index.Data);
            }
        }

        return availableData;
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

        criteria.Ssm = new MutationCriteria { Id = new[] { variantId } };

        // Should never be null or empty
        var ids = AggregateFromVariants(index => index.Samples.First().Donor.Id, criteria);

        criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };

        var filters = new DonorIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(donor => donor.NumberOfGenes)
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
