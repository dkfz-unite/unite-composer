using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Controllers.Search.Mutations
{
    [Route("api/[controller]")]
    public class MutationsController : Controller
    {
        private readonly ISearchService<MutationIndex> _searchService;


        public MutationsController(ISearchService<MutationIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public SearchResult<MutationResource> Get()
        {
            var searchResult = _searchService.Search();

            return From(searchResult);
        }

        [HttpPost]
        [CookieAuthorize]
        public SearchResult<MutationResource> Post([FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.Search(searchCriteria);

            return From(searchResult);
        }


        private SearchResult<MutationResource> From(SearchResult<MutationIndex> searchResult)
        {
            return new SearchResult<MutationResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new MutationResource(index)).ToArray()
            };
        }
    }
}
