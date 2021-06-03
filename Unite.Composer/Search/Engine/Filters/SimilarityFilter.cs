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

        protected Expression<Func<TIndex, TProp>> _property;
        protected IEnumerable<string> _values;


        public SimilarityFilter(string name, Expression<Func<TIndex, TProp>> property, IEnumerable<string> values)
        {
            Name = name;

            _property = property;
            _values = values;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddMatchQuery(_property, _values);
        }
    }
}
