using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Datasets;
using Unite.Composer.Data.Datasets.Models;
using Unite.Composer.Web.Extensions;

namespace Unite.Composer.Web.Controllers.Data.DataSets;

[Route("api/data/[controller]")]
[ApiController]
[Authorize]
public class DatasetController : Controller
{
    private readonly DatasetService _datasetService;
    private readonly DatasetsService _datasetsService;

    public DatasetController(DatasetService datasetService, DatasetsService datasetsService)
    {
        _datasetService = datasetService;
        _datasetsService = datasetsService;
    }


    [HttpPost()]
    public async Task<ActionResult<string>> Add([FromBody] DatasetModel dataset)
    {
        var email = User.GetUserEmail();
        if (email == null)
            return Forbid();

        dataset.UserId = email;
        return await _datasetService.Add(dataset);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var email = User.GetUserEmail();
        if (email == null)
            return Forbid();

        var datasets = await _datasetsService.Load(new SearchModel { UserId = email });
        //Check if dataset belongs to the user
        if (datasets.All(dataset => dataset.Id != id))
            return NotFound();

        await _datasetService.Delete(id);
        return Ok();
    }
}
