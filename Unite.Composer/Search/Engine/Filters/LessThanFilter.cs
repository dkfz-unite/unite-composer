using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class LessThanFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }

    public Expression<Func<TIndex, TProp>>[] Properties { get; }
    public double? Value { get; }

    
    public LessThanFilter(string name, double? value, params Expression<Func<TIndex, TProp>>[] properties)
    {
        Name = name;

        Properties = properties;
        Value = value;
    }

    public void Apply(ISearchRequest<TIndex> request)
    {
        request.AddLessThanQuery(Value, Properties);
    }
}
