using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices.Services;
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
        public MutationIndex Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return _indexService.Find(id);
        }
    }
}
