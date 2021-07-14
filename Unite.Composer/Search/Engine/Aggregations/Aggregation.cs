using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Aggregations
{
    public class Aggregation<TIndex, TProp> : IAggregation<TIndex> where TIndex : class
    {
        protected Expression<Func<TIndex, TProp>> _property;
        protected int? _size;

        public string Name { get; }


        public Aggregation(string name, Expression<Func<TIndex, TProp>> property, int? size)
        {
            Name = name;

            _property = property;
            _size = size;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddTermsAggregation(Name, _property, _size);
        }
    }
}
