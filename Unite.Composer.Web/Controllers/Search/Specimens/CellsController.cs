using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Search.Specimens
{
    [Route("api/[controller]")]
    public class CellsController : Controller
    {
        private readonly ISearchService<SpecimenIndex, SpecimenSearchContext> _searchService;


        public CellsController(ISearchService<SpecimenIndex, SpecimenSearchContext> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public SearchResult<SpecimenResource> Get()
        {
            var searchCriteria = new SearchCriteria();

            var searchContext = new SpecimenSearchContext(SpecimenType.CellLine);

            var searchResult = _searchService.Search(searchCriteria, searchContext);

            return From(searchResult);
        }

        [HttpPost]
        [CookieAuthorize]
        public SearchResult<SpecimenResource> Post([FromBody] SearchCriteria searchCriteria)
        {
            var searchContext = new SpecimenSearchContext(SpecimenType.CellLine);

            var searchResult = _searchService.Search(searchCriteria, searchContext);

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
