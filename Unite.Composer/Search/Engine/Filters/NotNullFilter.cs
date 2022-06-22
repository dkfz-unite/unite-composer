using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Search.Engine.Filters;

public class NotNullFilter<TIndex, TProp> : IFilter<TIndex> where TIndex : class
{
    public string Name { get; }

    public Expression<Func<TIndex, TProp>> Property { get; }


    public NotNullFilter(string name, Expression<Func<TIndex, TProp>> property)
    {
        Name = name;

        Property = property;
    }


    public void Apply(Nest.ISearchRequest<TIndex> request)
    {
        request.AddExistsQuery(Property);
    }
}
