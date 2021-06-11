using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Services
{
    public class SpecimensSearchService : ISearchService<SpecimenIndex, SpecimenSearchContext>
    {
        private readonly IIndexService<SpecimenIndex> _indexService;


        public SpecimensSearchService(IElasticOptions options)
        {
            _indexService = new SpecimensIndexService(options);
        }


        public SpecimenIndex Get(string key, SpecimenSearchContext searchContext = null)
        {
            var query = new GetQuery<SpecimenIndex>(key)
                .AddExclusion(specimen => specimen.Mutations);

            var result = _indexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<SpecimenIndex> Search(SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var context = searchContext ?? new SpecimenSearchContext();

            var filters = GetFiltersCollection(criteria, context)
                .All();

            var query = new SearchQuery<SpecimenIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(filters)
                .AddOrdering(specimen => specimen.NumberOfMutations)
                .AddExclusion(specimen => specimen.Mutations);

            var result = _indexService.SearchAsync(query).Result;

            return result;
        }


        private CriteriaFiltersCollection<SpecimenIndex> GetFiltersCollection(SearchCriteria criteria, SpecimenSearchContext context)
        {
            if (context.SpecimenType == Context.Enums.SpecimenType.Tissue)
            {
                return new TissueCriteriaFiltersCollection(criteria);
            }
            else if (context.SpecimenType == Context.Enums.SpecimenType.CellLine)
            {
                return new CellLineCriteriaFiltersCollection(criteria);
            }
            else
            {
                return new SpecimenCriteriaFiltersCollection(criteria); 
            }
        }
    }
}
