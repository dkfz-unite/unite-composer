using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class StructuralVariantFilters<TIndex> : VariantFilters<TIndex> where TIndex : class
{
    public StructuralVariantFilters(StructuralVariantCriteria criteria, Expression<Func<TIndex, VariantIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            StructuralVariantFilterNames.Type,
            path.Join(variant => variant.StructuralVariant.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        Add(new BooleanFilter<TIndex>(
            StructuralVariantFilterNames.Inverted,
            path.Join(variant => variant.StructuralVariant.Inverted),
            criteria.Inverted)
        );
    }
}
