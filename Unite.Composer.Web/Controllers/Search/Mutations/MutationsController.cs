using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Mutations;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Controllers.Search.Mutations
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutationsController : Controller
    {
        private readonly ISearchService<MutationIndex> _searchService;


        public MutationsController(ISearchService<MutationIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpPost("")]
        [CookieAuthorize]
        public SearchResult<MutationResource> Search([FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.Search(searchCriteria);

            return From(searchResult);
        }


        private static SearchResult<MutationResource> From(SearchResult<MutationIndex> searchResult)
        {
            return new SearchResult<MutationResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new MutationResource(index)).ToArray()
            };
        }
    }
}
