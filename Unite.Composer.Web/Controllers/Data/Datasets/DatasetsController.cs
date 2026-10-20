using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Datasets;
using Unite.Composer.Data.Datasets.Models;
using Unite.Composer.Web.Extensions;

namespace Unite.Composer.Web.Controllers.Data.DataSets;

[Route("api/data/[controller]")]
[ApiController]
[Authorize]
public class DatasetsController : Controller
{
    private readonly DatasetsService _datasetsService;


    public DatasetsController(DatasetsService datasetsService)
    {
        _datasetsService = datasetsService;
    }


    [HttpPost]
    public async Task<ActionResult<DatasetModel[]>> Load()
    {
        var email = User.GetUserEmail();
        if (email == null)
            return Forbid();

        var model = new SearchModel { UserId = email };
        return await _datasetsService.Load(model);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var email = User.GetUserEmail();
        if (email == null)
            return Forbid();

        var model = new SearchModel { UserId = email };
        await _datasetsService.Delete(model);
        return Ok();
    }
}
