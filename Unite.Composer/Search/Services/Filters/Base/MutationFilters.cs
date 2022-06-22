using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Mutations;

namespace Unite.Composer.Search.Services.Filters.Base;

public class MutationFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public MutationFilters(MutationCriteria criteria, Expression<Func<TIndex, MutationIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, long>(
            MutationFilterNames.Id,
            path.Join(mutation => mutation.Id),
            criteria.Id)
        );

        Add(new SimilarityFilter<TIndex, string>(
            MutationFilterNames.Code,
            path.Join(mutation => mutation.Code),
            criteria.Code)
        );

        Add(new EqualityFilter<TIndex, object>(
            MutationFilterNames.Type,
            path.Join(mutation => mutation.Type.Suffix(_keywordSuffix)),
            criteria.MutationType)
        );

        Add(new EqualityFilter<TIndex, object>(
            MutationFilterNames.Chromosome,
            path.Join(mutation => mutation.Chromosome.Suffix(_keywordSuffix)),
            criteria.Chromosome)
        );

        Add(new MultiPropertyRangeFilter<TIndex, int>(
            MutationFilterNames.Position,
            path.Join(mutation => mutation.Start),
            path.Join(mutation => mutation.End),
            criteria.Position?.From,
            criteria.Position?.To)
        );

        Add(new EqualityFilter<TIndex, object>(
            MutationFilterNames.Impact,
            path.Join(mutation => mutation.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
            criteria.Impact)
        );

        Add(new EqualityFilter<TIndex, object>(
            MutationFilterNames.Consequence,
            path.Join(mutation => mutation.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix)),
            criteria.Consequence)
        );
    }
}
