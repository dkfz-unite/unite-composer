using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class VariantFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public VariantFilters(VariantCriteriaBase criteria, Expression<Func<TIndex, VariantIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<TIndex, object>(
                VariantFilterNames.Id,
                path.Join(variant => variant.Id.Suffix(_keywordSuffix)),
                criteria.Id)
            );
        }
    }
}
