using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers.Search
{
    [Route("api/[controller]")]
    public class DonorController : Controller
    {
        private readonly ISearchService<DonorIndex> _searchService;


        public DonorController(ISearchService<DonorIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public DonorResource Get(int id)
        {
            var key = id.ToString();

            var index = _searchService.Get(key);

            return From(index);
        }


        private DonorResource From(DonorIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new DonorResource(index);
        }
    }
}
