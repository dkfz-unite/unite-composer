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
            VariantFilterNames.Chromosome,
            path.Join(variant => variant.Sv.Chromosome.Suffix(_keywordSuffix)),
            criteria.Chromosome)
        );

        Add(new MultiPropertyRangeFilter<TIndex, int>(
            VariantFilterNames.Position,
            path.Join(variant => variant.Sv.Start),
            path.Join(variant => variant.Sv.End),
            criteria.Position?.From,
            criteria.Position?.To)
        );

        Add(new RangeFilter<TIndex, int?>(
            VariantFilterNames.Length,
            path.Join(variant => variant.Sv.Length),
            criteria.Length?.From,
            criteria.Length?.To)
        );

        Add(new EqualityFilter<TIndex, object>(
            StructuralVariantFilterNames.Type,
            path.Join(variant => variant.Sv.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        Add(new BooleanFilter<TIndex>(
            StructuralVariantFilterNames.Inverted,
            path.Join(variant => variant.Sv.Inverted),
            criteria.Inverted)
        );

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Impact,
            path.Join(variant => variant.Sv.AffectedFeatures.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
            criteria.Impact)
        );

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Consequence,
            path.Join(variant => variant.Sv.AffectedFeatures.First().Consequences.First().Type.Suffix(_keywordSuffix)),
            criteria.Consequence)
        );
    }
}
