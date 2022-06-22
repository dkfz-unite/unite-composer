using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;

namespace Unite.Composer.Search.Services;

public interface ISearchService<TIndex>
    where TIndex : class
{
    TIndex Get(string key);
    SearchResult<TIndex> Search(SearchCriteria searchCriteria = null);
}


public interface ISearchService<TIndex, TContext>
    where TIndex : class
    where TContext : class
{
    TIndex Get(string key, TContext searchContext = null);
    SearchResult<TIndex> Search(SearchCriteria searchCriteria = null, TContext searchContext = null);
}
