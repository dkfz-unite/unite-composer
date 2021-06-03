using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class BooleanFilter<TIndex> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }

        protected Expression<Func<TIndex, bool?>> _property;
        protected bool? _value;


        public BooleanFilter(string name, Expression<Func<TIndex, bool?>> property, bool? value)
        {
            Name = name;

            _property = property;
            _value = value;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddBoolQuery(_property, _value);
        }
    }
}
