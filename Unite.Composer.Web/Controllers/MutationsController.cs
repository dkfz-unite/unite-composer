using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Controllers
{
    [Route("api/[controller]")]
    public class MutationsController : Controller
    {
        private readonly IIndexService<MutationIndex> _indexService;

        public MutationsController(IIndexService<MutationIndex> indexService)
        {
            _indexService = indexService;
        }

        [HttpGet]
        [CookieAuthorize]
        public SearchResult<MutationResource> Get()
        {
            var searchResult = _indexService.FindAll();

            return From(searchResult);
        }

        [HttpPost]
        [CookieAuthorize]
        public SearchResult<MutationResource> Post([FromBody] SearchCriteria criteria = null)
        {
            var searchResult = _indexService.FindAll(criteria);

            return From(searchResult);
        }


        private SearchResult<MutationResource> From(SearchResult<MutationIndex> searchResult)
        {
            return new SearchResult<MutationResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new MutationResource(index))
            };
        }
    }
}
