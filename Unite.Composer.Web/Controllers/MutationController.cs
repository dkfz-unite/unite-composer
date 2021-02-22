using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices.Services;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Controllers
{
    [Route("api/[controller]")]
    public class MutationController : Controller
    {
        private readonly IIndexService<MutationIndex> _indexService;

        public MutationController(IIndexService<MutationIndex> indexService)
        {
            _indexService = indexService;
        }

        [HttpGet]
        [CookieAuthorize]
        public MutationResource Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var index = _indexService.Find(id);

            return From(index);
        }

        private MutationResource From(MutationIndex index)
        {
            if(index == null)
            {
                return null;
            }

            return new MutationResource(index);
        }
    }
}
