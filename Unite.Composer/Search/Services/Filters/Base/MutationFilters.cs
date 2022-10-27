using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class MutationFilters<TIndex> : VariantFilters<TIndex> where TIndex : class //FiltersCollection<TIndex> where TIndex : class
{
    public MutationFilters(MutationCriteria criteria, Expression<Func<TIndex, VariantIndex>> path) : base(criteria, path)
    {
        //if (criteria == null)
        //{
        //    return;
        //}

        //Add(new EqualityFilter<TIndex, object>(
        //    MutationFilterNames.Chromosome,
        //    path.Join(variant => variant.Mutation.Chromosome.Suffix(_keywordSuffix)),
        //    criteria.Chromosome)
        //);

        //Add(new MultiPropertyRangeFilter<TIndex, int>(
        //    MutationFilterNames.Position,
        //    path.Join(variant => variant.Mutation.Start),
        //    path.Join(variant => variant.Mutation.End),
        //    criteria.Position?.From,
        //    criteria.Position?.To)
        //);

        Add(new EqualityFilter<TIndex, object>(
            MutationFilterNames.Type,
            path.Join(variant => variant.Mutation.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        //Add(new EqualityFilter<TIndex, object>(
        //    MutationFilterNames.Impact,
        //    path.Join(variant => variant.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
        //    criteria.Impact)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    MutationFilterNames.Consequence,
        //    path.Join(variant => variant.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix)),
        //    criteria.Consequence)
        //);
    }
}
