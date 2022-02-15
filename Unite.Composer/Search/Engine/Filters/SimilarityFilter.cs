using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class SimilarityFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }

        public Expression<Func<TIndex, TProp>> Property { get; }
        public IEnumerable<string> Values { get; }


        public SimilarityFilter(string name, Expression<Func<TIndex, TProp>> property, IEnumerable<string> values)
        {
            Name = name;

            Property = property;
            Values = values;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddMatchQuery(Property, Values);
        }
    }
}
