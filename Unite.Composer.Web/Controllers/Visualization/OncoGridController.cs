using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Visualization.Oncogrid.Data;

namespace Unite.Composer.Web.Controllers.Visualization;

[Route("api/[controller]")]
[Authorize]
public class OncoGridController : Controller
{
    private readonly OncoGridDataService1 _dataService;

    public OncoGridController(OncoGridDataService1 dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public OncoGridData Get()
    {
        return _dataService.LoadData();
    }

    [HttpPost]
    public OncoGridData Post([FromBody] SearchCriteria searchCriteria)
    {
        return _dataService.LoadData(searchCriteria);
    }
}
