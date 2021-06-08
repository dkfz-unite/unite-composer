using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;

namespace Unite.Composer.Search.Services
{
    public interface ISearchService<TIndex> where TIndex : class
    {
        TIndex Get(string key);
        SearchResult<TIndex> Search(SearchCriteria searchCriteria = null);
    }
}