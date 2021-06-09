using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Search.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Controllers.Search.Mutations
{
    [Route("api/[controller]")]
    public class MutationController : Controller
    {
        private readonly ISearchService<MutationIndex> _searchService;


        public MutationController(ISearchService<MutationIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public MutationResource Get(long id)
        {
            var key = id.ToString();

            var index = _searchService.Get(key);

            return From(index);
        }


        private MutationResource From(MutationIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new MutationResource(index);
        }
    }
}
