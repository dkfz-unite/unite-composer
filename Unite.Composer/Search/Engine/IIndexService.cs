using Unite.Composer.Search.Engine.Queries;

namespace Unite.Composer.Search.Engine;

public interface IIndexService<TIndex>
    where TIndex : class
{
    Task<TIndex> GetAsync(GetQuery<TIndex> query);
    Task<SearchResult<TIndex>> SearchAsync(SearchQuery<TIndex> query);
}
