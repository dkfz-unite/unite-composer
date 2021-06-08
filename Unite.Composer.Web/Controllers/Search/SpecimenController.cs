using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Search
{
    [Route("api/[controller]")]
    public class SpecimenController : Controller
    {
        private readonly ISearchService<SpecimenIndex> _searchService;


        public SpecimenController(ISearchService<SpecimenIndex> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public SpecimenResource Get(int id)
        {
            var key = id.ToString();

            var index = _searchService.Get(key);

            return From(index);
        }


        private SpecimenResource From(SpecimenIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new SpecimenResource(index);
        }
    }
}
