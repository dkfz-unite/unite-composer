using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Search.Specimens
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecimensController : Controller
    {
        private readonly ISpecimensSearchService _searchService;


        public SpecimensController(ISpecimensSearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpPost("{type}")]
        public SearchResult<SpecimenResource> Search(SpecimenType type, [FromBody] SearchCriteria searchCriteria)
        {
            var searchContext = new SpecimenSearchContext(type);

            var searchResult = _searchService.Search(searchCriteria, searchContext);

            return From(searchResult);
        }


        private static SearchResult<SpecimenResource> From(SearchResult<SpecimenIndex> searchResult)
        {
            return new SearchResult<SpecimenResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new SpecimenResource(index)).ToArray()
            };
        }
    }
}
