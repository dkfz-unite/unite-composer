using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class StructuralVariantFilters<TIndex> : VariantFilters<TIndex> where TIndex : class //FiltersCollection<TIndex> where TIndex : class
{
    public StructuralVariantFilters(StructuralVariantCriteria criteria, Expression<Func<TIndex, VariantIndex>> path) : base(criteria, path)
    {
        //if (criteria == null)
        //{
        //    return;
        //}

        //Add(new EqualityFilter<TIndex, object>(
        //    StructuralVariantFilterNames.Chromosome,
        //    path.Join(variant => variant.StructuralVariant.Chromosome.Suffix(_keywordSuffix)),
        //    criteria.Chromosome)
        //);

        //Add(new MultiPropertyRangeFilter<TIndex, int>(
        //    StructuralVariantFilterNames.Position,
        //    path.Join(variant => variant.StructuralVariant.Start),
        //    path.Join(variant => variant.StructuralVariant.End),
        //    criteria.Position?.From,
        //    criteria.Position?.To)
        //);

        Add(new EqualityFilter<TIndex, object>(
            StructuralVariantFilterNames.Type,
            path.Join(variant => variant.StructuralVariant.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        //Add(new EqualityFilter<TIndex, object>(
        //    StructuralVariantFilterNames.Impact,
        //    path.Join(variant => variant.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
        //    criteria.Impact)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    StructuralVariantFilterNames.Consequence,
        //    path.Join(variant => variant.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix)),
        //    criteria.Consequence)
        //);
    }
}
