using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers
{
    [Route("api/[controller]")]
    public class DonorsController : Controller
    {
        private readonly IIndexService<DonorIndex> _indexService;

        public DonorsController(IIndexService<DonorIndex> indexService)
        {
            _indexService = indexService;
        }

        [HttpGet]
        [CookieAuthorize]
        public SearchResult<DonorResource> Get()
        {
            var searchResult = _indexService.FindAll();

            return From(searchResult);
        }

        [HttpPost]
        [CookieAuthorize]
        public SearchResult<DonorResource> Post([FromBody]SearchCriteria criteria = null)
        {
            var searchResult = _indexService.FindAll(criteria);

            return From(searchResult);
        }

        private SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
        {
            return new SearchResult<DonorResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorResource(index))
            };
        }
    }
}
