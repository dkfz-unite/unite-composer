using Nest;

namespace Unite.Composer.Search.Engine.Aggregations
{
    public interface IAggregation<TIndex>
        where TIndex : class
    {
        string Name { get; }

        void Apply(ISearchRequest<TIndex> request);
    }
}
