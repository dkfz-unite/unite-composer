using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Visualization.Oncogrid.Models;
using Unite.Composer.Web.Configuration.Filters.Attributes;

namespace Unite.Composer.Web.Controllers.Visualization
{
    [Route("api/[controller]")]
    public class OncogridController : Controller
    {
        private readonly OncogridDataService _dataService;


        public OncogridController(OncogridDataService dataService)
        {
            _dataService = dataService;
        }


        [HttpGet]
        [CookieAuthorize]
        public OncogridData Get()
        {
            return _dataService.GetData();
        }


        [HttpPost]
        [CookieAuthorize]
        public OncogridData Post([FromBody] SearchCriteria searchCriteria)
        {
            return _dataService.GetData(searchCriteria);
        }
    }
}
