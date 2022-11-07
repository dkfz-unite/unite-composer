using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class MutationFilters<TIndex> : VariantFilters<TIndex> where TIndex : class
{
    public MutationFilters(MutationCriteria criteria, Expression<Func<TIndex, VariantIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            MutationFilterNames.Type,
            path.Join(variant => variant.Mutation.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );
    }
}
