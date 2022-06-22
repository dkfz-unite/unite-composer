using Nest;

namespace Unite.Composer.Search.Engine.Filters;

public interface IFilter<TIndex> where TIndex : class
{
    string Name { get; }

    void Apply(ISearchRequest<TIndex> request);
}
