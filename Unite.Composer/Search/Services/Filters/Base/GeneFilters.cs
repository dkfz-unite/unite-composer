using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Search.Services.Filters.Base
{
    public class GeneFilters<TIndex> : FiltersCollection<TIndex>
        where TIndex : class
    {
        public GeneFilters(GeneCriteria criteria, Expression<Func<TIndex, GeneIndex>> path)
        {
            if (criteria == null)
            {
                return;
            }

            _filters.Add(new EqualityFilter<TIndex, int>(
                GeneFilterNames.Id,
                path.Join(gene => gene.Id),
                criteria.Id)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                GeneFilterNames.Symbol,
                path.Join(gene => gene.Symbol),
                criteria.Symbol)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                GeneFilterNames.Biotype,
                path.Join(gene => gene.Biotype.Suffix(_keywordSuffix)),
                criteria.Biotype)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                GeneFilterNames.Chromosome,
                path.Join(gene => gene.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome)
            );

            _filters.Add(new MultiPropertyRangeFilter<TIndex, int?>(
                GeneFilterNames.Position,
                path.Join(gene => gene.Start),
                path.Join(gene => gene.End),
                criteria.Position?.From,
                criteria.Position?.To)
            );
        }
    }
}
