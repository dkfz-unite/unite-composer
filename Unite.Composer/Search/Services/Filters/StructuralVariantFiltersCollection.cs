using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Search.Services.Filters;

public class StructuralVariantFiltersCollection : VariantFiltersCollection
{
    public StructuralVariantFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new StructuralVariantFilters<VariantIndex>(criteria.Sv, variant => variant);
        var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Sv.AffectedFeatures.First().Gene);

        _filters.AddRange(filters.All());
        _filters.AddRange(geneFilters.All());

        Add(new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.StructuralVariantIndex>(
            VariantFilterNames.Type,
            variant => variant.Sv
        ));
    }
}
