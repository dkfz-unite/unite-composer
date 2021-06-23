using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class EqualityFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }

        protected Expression<Func<TIndex, TProp>> _property;
        protected IEnumerable<TProp> _values;


        public EqualityFilter(string name, Expression<Func<TIndex, TProp>> property, IEnumerable<TProp> values)
        {
            Name = name;

            _property = property;
            _values = values;
        }

        public EqualityFilter(string name, Expression<Func<TIndex, TProp>> property, TProp value)
        {
            Name = name;

            _property = property;
            _values = new TProp[] { value };
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddTermsQuery(_property, _values);
        }
    }
}
