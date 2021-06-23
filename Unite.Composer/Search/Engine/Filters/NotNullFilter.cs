using System;
using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters
{
    public class NotNullFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
    {
        public string Name { get; }

        protected Expression<Func<TIndex, TProp>> _property;


        public NotNullFilter(string name, Expression<Func<TIndex, TProp>> property)
        {
            Name = name;

            _property = property;
        }


        public void Apply(Nest.ISearchRequest<TIndex> request)
        {
            request.AddExistsQuery(_property);
        }
    }
}
