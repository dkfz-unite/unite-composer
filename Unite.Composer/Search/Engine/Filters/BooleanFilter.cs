using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class BooleanFilter<TIndex> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }

        public Expression<Func<TIndex, bool?>> Property { get; }
        public bool? Value { get; }


        public BooleanFilter(string name, Expression<Func<TIndex, bool?>> property, bool? value)
        {
            Name = name;

            Property = property;
            Value = value;
        }


        public void Apply(ISearchRequest<TIndex> request)
        {
            request.AddBoolQuery(Property, Value);
        }
    }
}
