using Nest;

namespace Unite.Composer.Search.Engine.Filters
{
    public interface IFilter<TIndex>
        where TIndex : class
    {
        public string Name { get; }

        public void Apply(ISearchRequest<TIndex> request);
    }
}
