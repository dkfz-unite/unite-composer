using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Indices.Entities.Variants;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public interface IVariantsSearchService : ISearchService<VariantIndex, VariantSearchContext>
{
    IDictionary<long, DataIndex> Stats(SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null);
    SearchResult<DonorIndex> SearchDonors(string variantId, SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null);
}
