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
    public async Task<ActionResult<DatasetModel[]>> Load([FromBody] SearchModel model)
    {
        // Root can load datasets of any user, others only their own
        if (!User.GetIsRoot() || String.IsNullOrEmpty(model.UserId))
        {
            var email = User.GetUserEmail();
            if (email == null)
                return Forbid();

            model.UserId = email;
        }

        return await _datasetsService.Load(model);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string userId)
    {
        var localUserId = userId;
        // Root can delete datasets of any user, others only their own
        if (!User.GetIsRoot() || String.IsNullOrEmpty(localUserId))
        {
            localUserId = User.GetUserEmail();
            if (localUserId == null)
                return Forbid();
        }

        var model = new SearchModel { UserId = localUserId };
        await _datasetsService.Delete(model);
        return Ok();
    }
}
