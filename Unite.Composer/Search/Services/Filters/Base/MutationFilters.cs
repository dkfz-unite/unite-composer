﻿using System.Linq.Expressions;
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

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<TIndex, object>(
                VariantFilterNames.Chromosome,
                path.Join(variant => variant.Ssm.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome)
            );
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<TIndex, int>(
                VariantFilterNames.Position,
                path.Join(variant => variant.Ssm.Start),
                path.Join(variant => variant.Ssm.End),
                criteria.Position?.From,
                criteria.Position?.To)
            );
        }

        if (IsNotEmpty(criteria.Length))
        {
            Add(new RangeFilter<TIndex, int>(
                VariantFilterNames.Length,
                path.Join(variant => variant.Ssm.Length),
                criteria.Length?.From,
                criteria.Length?.To)
            );
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<TIndex, object>(
                MutationFilterNames.Type,
                path.Join(variant => variant.Ssm.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );
        }

        if (IsNotEmpty(criteria.Impact))
        {
            Add(new EqualityFilter<TIndex, object>(
                VariantFilterNames.Impact,
                path.Join(variant => variant.Ssm.AffectedFeatures.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
                criteria.Impact)
            );
        }

        if (IsNotEmpty(criteria.Consequence))
        {
            Add(new EqualityFilter<TIndex, object>(
                VariantFilterNames.Consequence,
                path.Join(variant => variant.Ssm.AffectedFeatures.First().Consequences.First().Type.Suffix(_keywordSuffix)),
                criteria.Consequence)
            );
        }
    }
}
