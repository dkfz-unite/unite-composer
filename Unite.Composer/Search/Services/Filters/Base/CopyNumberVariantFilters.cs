﻿using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Search.Services.Filters.Base;

public class CopyNumberVariantFilters<TIndex> : VariantFilters<TIndex> where TIndex : class //FiltersCollection<TIndex> where TIndex : class
{
    public CopyNumberVariantFilters(CopyNumberVariantCriteria criteria, Expression<Func<TIndex, VariantIndex>> path) : base(criteria, path)
    {
        //if (criteria == null)
        //{
        //    return;
        //}

        //Add(new EqualityFilter<TIndex, object>(
        //    CopyNumberVariantFilterNames.Chromosome,
        //    path.Join(variant => variant.CopyNumberVariant.Chromosome.Suffix(_keywordSuffix)),
        //    criteria.Chromosome)
        //);

        //Add(new MultiPropertyRangeFilter<TIndex, int>(
        //    CopyNumberVariantFilterNames.Position,
        //    path.Join(variant => variant.CopyNumberVariant.Start),
        //    path.Join(variant => variant.CopyNumberVariant.End),
        //    criteria.Position?.From,
        //    criteria.Position?.To)
        //);

        Add(new EqualityFilter<TIndex, object>(
            CopyNumberVariantFilterNames.SvType,
            path.Join(variant => variant.CopyNumberVariant.SvType.Suffix(_keywordSuffix)),
            criteria.SvType)
        );

        Add(new EqualityFilter<TIndex, object>(
            CopyNumberVariantFilterNames.CnaType,
            path.Join(variant => variant.CopyNumberVariant.CnaType.Suffix(_keywordSuffix)),
            criteria.CnaType)
        );

        Add(new BooleanFilter<TIndex>(
            CopyNumberVariantFilterNames.Loh,
            path.Join(variant => variant.CopyNumberVariant.Loh),
            criteria.Loh)
        );

        Add(new BooleanFilter<TIndex>(
            CopyNumberVariantFilterNames.HomoDel,
            path.Join(variant => variant.CopyNumberVariant.HomoDel),
            criteria.HomoDel)
        );

        //Add(new EqualityFilter<TIndex, object>(
        //    CopyNumberVariantFilterNames.Impact,
        //    path.Join(variant => variant.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
        //    criteria.Impact)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    CopyNumberVariantFilterNames.Consequence,
        //    path.Join(variant => variant.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix)),
        //    criteria.Consequence)
        //);
    }
}