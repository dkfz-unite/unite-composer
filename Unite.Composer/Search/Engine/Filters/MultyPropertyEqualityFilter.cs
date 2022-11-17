using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class MultyPropertyEqualityFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }

    public Expression<Func<TIndex, TProp>> Property1 { get; }
    public Expression<Func<TIndex, TProp>> Property2 { get; }
    public Expression<Func<TIndex, TProp>> Property3 { get; }
    public IEnumerable<TProp> Values { get; }


    public MultyPropertyEqualityFilter(
        string name,
        Expression<Func<TIndex, TProp>> property1,
        Expression<Func<TIndex, TProp>> property2,
        Expression<Func<TIndex, TProp>> property3,
        IEnumerable<TProp> values)
    {
        Name = name;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = values;
    }

    public MultyPropertyEqualityFilter(
        string name,
        Expression<Func<TIndex, TProp>> property1,
        Expression<Func<TIndex, TProp>> property2,
        Expression<Func<TIndex, TProp>> property3,
        TProp value)
    {
        Name = name;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = new TProp[] { value };
    }


    public void Apply(ISearchRequest<TIndex> request)
    {
        request.AddTermsQuery(Property1, Property2, Property3, Values);
    }
}
