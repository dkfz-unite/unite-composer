using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers.Search.Donors
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorsController : Controller
    {
        private readonly IDonorsSearchService _donorsSearchService;


        public DonorsController(IDonorsSearchService donorsSearchService)
        {
            _donorsSearchService = donorsSearchService;
        }


        [HttpPost("")]
        [CookieAuthorize]
        public SearchResult<DonorResource> Search([FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _donorsSearchService.Search(searchCriteria);

            return From(searchResult);
        }


        private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
        {
            return new SearchResult<DonorResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
            };
        }
    }
}
