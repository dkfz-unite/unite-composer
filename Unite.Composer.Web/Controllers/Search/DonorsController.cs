using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers.Search
{
    [Route("api/[controller]")]
    public class DonorsController : Controller
    {
        private readonly ISearchService<DonorIndex> _searchService;


        public DonorsController(ISearchService<DonorIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public SearchResult<DonorResource> Get()
        {
            var searchResult = _searchService.Search();

            return From(searchResult);
        }

        [HttpPost]
        [CookieAuthorize]
        public SearchResult<DonorResource> Post([FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.Search(searchCriteria);

            return From(searchResult);
        }


        private SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
        {
            return new SearchResult<DonorResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
            };
        }
    }
}
