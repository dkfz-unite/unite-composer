using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Services
{
    public class DonorsSearchService : ISearchService<DonorIndex>
    {
        private readonly IIndexService<DonorIndex> _indexService;


        public DonorsSearchService(IElasticOptions options)
        {
            _indexService = new DonorsIndexService(options);
        }


        public DonorIndex Get(string key)
        {
            var query = new GetQuery<DonorIndex>(key)
                .AddExclusion(donor => donor.Mutations);

            var result = _indexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<DonorIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new DonorCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations)
                .AddExclusion(donor => donor.Mutations);

            var result = _indexService.SearchAsync(query).Result;

            return result;
        }
    }
}
