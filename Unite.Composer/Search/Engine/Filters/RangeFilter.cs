using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class RangeFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }

        protected Expression<Func<TIndex, TProp>> _property;
        protected double? _from;
        protected double? _to;


        public RangeFilter(string name, Expression<Func<TIndex, TProp>> property, double? from, double? to)
        {
            Name = name;

            _property = property;
            _from = from;
            _to = to;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddRangeQuery(_property, _from, _to);
        }
    }
}
