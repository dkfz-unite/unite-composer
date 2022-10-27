using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Search.Services.Filters;

public class CopyNumberVariantFiltersCollection : VariantFiltersCollection
{
    public CopyNumberVariantFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new CopyNumberVariantFilters<VariantIndex>(criteria.CopyNumberVariantFilters, variant => variant);

        _filters.AddRange(filters.All());

        Add(new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.CopyNumberVariantIndex>(
            VariantFilterNames.Type,
            variant => variant.CopyNumberVariant
        ));
    }
}
