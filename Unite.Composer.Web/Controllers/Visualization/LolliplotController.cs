using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Visualization.Lolliplot;

namespace Unite.Composer.Web.Controllers.Visualization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LolliplotController : Controller
{
    private readonly ProteinPlotDataService _dataService;

    public LolliplotController(ProteinPlotDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("transcript/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _dataService.LoadData(id);

        return Ok(result);
    }
}
