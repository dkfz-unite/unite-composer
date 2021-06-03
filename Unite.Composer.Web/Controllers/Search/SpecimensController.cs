using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Search
{
    [Route("api/[controller]")]
    public class SpecimensController : Controller
    {
        private readonly ISearchService<SpecimenIndex> _searchService;


        public SpecimensController(ISearchService<SpecimenIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public SearchResult<SpecimenResource> Get()
        {
            var searchResult = _searchService.Search();

            return From(searchResult);
        }

        [HttpPost]
        [CookieAuthorize]
        public SearchResult<SpecimenResource> Post([FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.Search(searchCriteria);

            return From(searchResult);
        }


        private SearchResult<SpecimenResource> From(SearchResult<SpecimenIndex> searchResult)
        {
            return new SearchResult<SpecimenResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new SpecimenResource(index)).ToArray()
            };
        }
    }
}
