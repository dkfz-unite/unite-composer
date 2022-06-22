using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class MultiPropertyRangeFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }


    public Expression<Func<TIndex, TProp>> PropertyFrom { get; }
    public Expression<Func<TIndex, TProp>> PropertyTo { get; }
    public double? ValueFrom { get; }
    public double? ValueTo { get; }


    public MultiPropertyRangeFilter(
        string name,
        Expression<Func<TIndex, TProp>> propertyFrom,
        Expression<Func<TIndex, TProp>> propertyTo,
        double? valueFrom,
        double? valueTo)
    {
        Name = name;

        PropertyFrom = propertyFrom;
        PropertyTo = propertyTo;
        ValueFrom = valueFrom;
        ValueTo = valueTo;
    }


    public void Apply(ISearchRequest<TIndex> request)
    {
        request.AddRangeQuery(PropertyFrom, PropertyTo, ValueFrom, ValueTo);
    }
}
