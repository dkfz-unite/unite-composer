using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Search.Specimens
{
    [Route("api/[controller]")]
    public class TissueController : Controller
    {
        private readonly ISearchService<SpecimenIndex, SpecimenSearchContext> _searchService;


        public TissueController(ISearchService<SpecimenIndex, SpecimenSearchContext> searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [CookieAuthorize]
        public SpecimenResource Get(int id)
        {
            var key = id.ToString();

            var context = new SpecimenSearchContext(SpecimenType.Tissue);

            var index = _searchService.Get(key, context);

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
