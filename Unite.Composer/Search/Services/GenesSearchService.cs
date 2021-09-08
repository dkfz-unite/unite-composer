using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Indices.Entities.Genes;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Services
{
    public class GenesSearchService : ISearchService<GeneIndex>
    {
        private readonly IIndexService<GeneIndex> _indexService;


        public GenesSearchService(IElasticOptions options)
        {
            _indexService = new GenesIndexService(options);
        }

        public GeneIndex Get(string key)
        {
            var query = new GetQuery<GeneIndex>(key)
                .AddExclusion(gene => gene.Mutations);

            var result = _indexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<GeneIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new GeneCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<GeneIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(gene => gene.NumberOfDonors)
                .AddExclusion(gene => gene.Mutations);

            var result = _indexService.SearchAsync(query).Result;

            return result;
        }
    }
}
