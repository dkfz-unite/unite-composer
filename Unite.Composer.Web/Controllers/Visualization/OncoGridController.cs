using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Composer.Web.Configuration.Filters.Attributes;

namespace Unite.Composer.Web.Controllers.Visualization
{
    [Route("api/[controller]")]
    public class OncoGridController : Controller
    {
        private readonly OncoGridDataService _dataService;

        public OncoGridController(OncoGridDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [CookieAuthorize]
        public OncoGridData Get()
        {
            return _dataService.GetData();
        }

        [HttpPost]
        [CookieAuthorize]
        public OncoGridData Post([FromBody] SearchCriteria searchCriteria)
        {
            return _dataService.GetData(searchCriteria);
        }
    }
}