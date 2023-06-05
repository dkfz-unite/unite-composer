using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class CopyNumberVariantFilters<TIndex> : VariantFilters<TIndex> where TIndex : class
{
    public CopyNumberVariantFilters(CopyNumberVariantCriteria criteria, Expression<Func<TIndex, VariantIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Chromosome,
            path.Join(variant => variant.Cnv.Chromosome.Suffix(_keywordSuffix)),
            criteria.Chromosome)
        );

        Add(new MultiPropertyRangeFilter<TIndex, int>(
            VariantFilterNames.Position,
            path.Join(variant => variant.Cnv.Start),
            path.Join(variant => variant.Cnv.End),
            criteria.Position?.From,
            criteria.Position?.To)
        );

        Add(new RangeFilter<TIndex, int>(
            VariantFilterNames.Length,
            path.Join(variant => variant.Cnv.Length),
            criteria.Length?.From,
            criteria.Length?.To)
        );

        Add(new EqualityFilter<TIndex, object>(
            CopyNumberVariantFilterNames.Type,
            path.Join(variant => variant.Cnv.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        Add(new BooleanFilter<TIndex>(
            CopyNumberVariantFilterNames.Loh,
            path.Join(variant => variant.Cnv.Loh),
            criteria.Loh)
        );

        Add(new BooleanFilter<TIndex>(
            CopyNumberVariantFilterNames.HomoDel,
            path.Join(variant => variant.Cnv.HomoDel),
            criteria.HomoDel)
        );

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Impact,
            path.Join(variant => variant.Cnv.AffectedFeatures.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
            criteria.Impact)
        );

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Consequence,
            path.Join(variant => variant.Cnv.AffectedFeatures.First().Consequences.First().Type.Suffix(_keywordSuffix)),
            criteria.Consequence)
        );
    }
}
