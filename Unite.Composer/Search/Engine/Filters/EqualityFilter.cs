using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class EqualityFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }

    public Expression<Func<TIndex, TProp>> Property { get; }
    public IEnumerable<TProp> Values { get; }


    public EqualityFilter(string name, Expression<Func<TIndex, TProp>> property, IEnumerable<TProp> values)
    {
        Name = name;

        Property = property;
        Values = values;
    }

    public EqualityFilter(string name, Expression<Func<TIndex, TProp>> property, TProp value)
    {
        Name = name;

        Property = property;
        Values = new TProp[] { value };
    }


    public void Apply(ISearchRequest<TIndex> request)
    {
        request.AddTermsQuery(Property, Values);
    }
}
