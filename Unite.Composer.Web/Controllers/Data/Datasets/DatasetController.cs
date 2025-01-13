using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Datasets;
using Unite.Composer.Data.Datasets.Models;

namespace Unite.Composer.Web.Controllers.Data.DataSets;

[Route("api/data/[controller]")]
[ApiController]
[Authorize]
public class DatasetController : Controller
{
 private readonly DatasetService _datasetService;

    public DatasetController(DatasetService datasetService)
    {
        _datasetService = datasetService;
    }

    [HttpPost()]
    public async Task<string> Add([FromBody] DatasetModel dataset)
    {
        return await _datasetService.Add(dataset);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await _datasetService.Delete(id);
    }

    [HttpDelete("{userId}/delete")]
    public async Task DeleteUser(string userId)
    {
        await _datasetService.DeleteUser(userId);
    }
}