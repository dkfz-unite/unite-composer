using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices.Services;
using Unite.Composer.Resources.Donors;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers
{
    [Route("api/[controller]")]
    public class DonorController : Controller
    {
        private readonly IIndexService<DonorIndex> _indexService;

        public DonorController(IIndexService<DonorIndex> indexService)
        {
            _indexService = indexService;
        }

        [HttpGet]
        [CookieAuthorize]
        public DonorResource Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var index = _indexService.Find(id);

            return From(index);
        }

        private DonorResource From(DonorIndex index)
        {
            if(index == null)
            {
                return null;
            }

            return new DonorResource(index);
        }
    }
}
