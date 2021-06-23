using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class MultiPropertyRangeFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }


        protected Expression<Func<TIndex, TProp>> _propertyFrom;
        protected Expression<Func<TIndex, TProp>> _propertyTo;
        protected double? _from;
        protected double? _to;


        public MultiPropertyRangeFilter(
            string name,
            Expression<Func<TIndex, TProp>> propertyFrom,
            Expression<Func<TIndex, TProp>> propertyTo,
            double? from,
            double? to)
        {
            Name = name;

            _propertyFrom = propertyFrom;
            _propertyTo = propertyTo;
            _from = from;
            _to = to;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddRangeQuery(_propertyFrom, _propertyTo, _from, _to);
        }
    }
}
