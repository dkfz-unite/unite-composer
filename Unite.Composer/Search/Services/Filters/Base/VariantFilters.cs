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

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Id,
            path.Join(variant => variant.Id.Suffix(_keywordSuffix)),
            criteria.Id)
        );

        //TODO: Add variant type filter

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Chromosome,
            path.Join(variant => variant.Chromosome.Suffix(_keywordSuffix)),
            criteria.Chromosome)
        );

        Add(new MultiPropertyRangeFilter<TIndex, int>(
            VariantFilterNames.Position,
            path.Join(variant => variant.Start),
            path.Join(variant => variant.End),
            criteria.Position?.From,
            criteria.Position?.To)
        );


        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Impact,
            path.Join(variant => variant.AffectedFeatures.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
            criteria.Impact)
        );

        Add(new EqualityFilter<TIndex, object>(
            VariantFilterNames.Consequence,
            path.Join(variant => variant.AffectedFeatures.First().Consequences.First().Type.Suffix(_keywordSuffix)),
            criteria.Consequence)
        );
    }
}
