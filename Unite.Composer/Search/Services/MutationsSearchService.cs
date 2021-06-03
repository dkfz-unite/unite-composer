using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Indices.Entities.Mutations;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Services
{
    public class MutationsSearchService : ISearchService<MutationIndex>
    {
        private readonly IIndexService<MutationIndex> _indexService;


        public MutationsSearchService(IElasticOptions options)
        {
            _indexService = new MutationsIndexService(options);
        }


        public MutationIndex Get(string key)
        {
            var query = new GetQuery<MutationIndex>(key)
                .AddExclusion(mutation => mutation.Donors);

            var result = _indexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<MutationIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new MutationCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<MutationIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(mutation => mutation.NumberOfDonors)
                .AddExclusion(mutation => mutation.Donors);

            var result = _indexService.SearchAsync(query).Result;

            return result;
        }
    }
}
