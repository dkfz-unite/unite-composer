using System.Threading.Tasks;
using Unite.Composer.Indices.Criteria;

namespace Unite.Composer.Indices.Services
{
    public interface IIndexService<T>
        where T : class
    {
        T Find(string key);
        SearchResult<T> FindAll(SearchCriteria searchCriteria = null);

        Task<T> FindAsync(string key);
        Task<SearchResult<T>> FindAllAsync(SearchCriteria searchCriteria = null);
    }
}
