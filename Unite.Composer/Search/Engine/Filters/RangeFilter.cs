using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class RangeFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }

    public Expression<Func<TIndex, TProp>> Property { get; }
    public double? From { get; }
    public double? To { get; }


    public RangeFilter(string name, Expression<Func<TIndex, TProp>> property, double? from, double? to)
    {
        Name = name;

        Property = property;
        From = from;
        To = to;
    }


    public void Apply(ISearchRequest<TIndex> request)
    {
        request.AddRangeQuery(Property, From, To);
    }
}
