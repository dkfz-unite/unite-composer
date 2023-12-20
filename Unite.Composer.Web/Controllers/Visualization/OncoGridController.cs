using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Visualization;

[Route("api/[controller]")]
[Authorize]
public class OncoGridController : Controller
{
    private readonly OncoGridDataService _dataService;

    public OncoGridController(OncoGridDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = _dataService.LoadData();

        var resource = Ok(result);

        return await Task.FromResult(resource);
    }

    [HttpPost]
    public async Task<IActionResult> Post(int donors, int genes, [FromBody]SearchCriteria searchCriteria)
    {
        var result = _dataService.LoadData(donors, genes, searchCriteria);

        var resource = Ok(result);

        return await Task.FromResult(resource);
    }
}
