using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Composer.Web.Configuration.Filters.Attributes;

namespace Unite.Composer.Web.Controllers.Visualization
{
    [Route("api/[controller]")]
    public class LolliplotController : Controller
    {
        private readonly LolliplotDataService _dataService;

        public LolliplotController(LolliplotDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [CookieAuthorize]
        public LolliplotData Get()
        {
            return _dataService.GetData();
        }

        [HttpPost]
        [CookieAuthorize]
        public LolliplotData Post([FromBody] SearchCriteria searchCriteria)
        {
            return _dataService.GetData(searchCriteria);
        }
    }
}