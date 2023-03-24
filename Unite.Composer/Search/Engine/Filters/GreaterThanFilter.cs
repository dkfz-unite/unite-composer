using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class GreaterThanFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }

    public Expression<Func<TIndex, TProp>>[] Properties { get; }
    public double? Value { get; }

    
    public GreaterThanFilter(string name, double? value, params Expression<Func<TIndex, TProp>>[] properties)
    {
        Name = name;

        Properties = properties;
        Value = value;
    }


    public void Apply(ISearchRequest<TIndex> request)
    {
        request.AddGreaterThanQuery(Value, Properties);
    }
}
