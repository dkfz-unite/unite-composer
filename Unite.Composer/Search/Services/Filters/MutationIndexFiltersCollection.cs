using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Search.Services.Filters;

public class MutationIndexFiltersCollection : VariantFiltersCollection
{
    public MutationIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new MutationFilters<VariantIndex>(criteria.MutationFilters, variant => variant);

        _filters.AddRange(filters.All());

        Add(new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.MutationIndex>(
            VariantFilterNames.Type,
            variant => variant.Mutation
        ));
    }
}
