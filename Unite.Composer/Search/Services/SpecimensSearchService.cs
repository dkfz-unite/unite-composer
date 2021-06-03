using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Services
{
    public class SpecimensSearchService : ISearchService<SpecimenIndex>
    {
        private readonly IIndexService<SpecimenIndex> _indexService;


        public SpecimensSearchService(IElasticOptions options)
        {
            _indexService = new SpecimensIndexService(options);
        }


        public SpecimenIndex Get(string key)
        {
            var query = new GetQuery<SpecimenIndex>(key)
                .AddExclusion(specimen => specimen.Mutations);

            var result = _indexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<SpecimenIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new SpecimenCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<SpecimenIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(specimen => specimen.NumberOfMutations)
                .AddExclusion(specimen => specimen.Mutations);

            var result = _indexService.SearchAsync(query).Result;

            return result;
        }
    }
}
