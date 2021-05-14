using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
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
        public DonorResource Get(int id)
        {
            var key = id.ToString();

            var index = _indexService.Find(key);

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
