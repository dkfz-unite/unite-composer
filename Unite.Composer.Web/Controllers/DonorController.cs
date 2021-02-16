using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices.Services;
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
        public DonorIndex Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return _indexService.Find(id);
        }
    }
}
