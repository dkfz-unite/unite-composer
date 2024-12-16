using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Datasets;
using Unite.Composer.Data.Datasets.Models;

namespace Unite.Composer.Web.Controllers.Data.DataSets;

[Route("api/data/[controller]")]
[ApiController]
[Authorize]
public class DatasetsController : Controller
{
    private readonly DatasetsService _datasetService;

    public DatasetsController(DatasetsService datasetService)
    {
        _datasetService = datasetService;
    }

    [HttpPost("addDataset")]
    public IActionResult AddDataset([FromBody] DatasetsModel dataset)
    {
        var datasetModel = new DatasetsModel
        {
           UserID = dataset.UserID,
           Domain = dataset.Domain,
           Name = dataset.Name,
           Description = dataset.Description,
           Date = dataset.Date,
           Criteria = dataset.Criteria
        };
        
        var response = _datasetService.AddDataset(datasetModel);
        return Ok(response);
    }

    [HttpPost("loadDatasets")]
    public IActionResult LoadDatasets([FromBody] DatasetsModel dataset)
    {
        var datasetModel = new DatasetsModel
        {
           UserID = dataset.UserID,
           Domain = dataset.Domain,
           Criteria = dataset.Criteria
        };
        
        var response = _datasetService.LoadDatasets(datasetModel);
        return Ok(response);
    }

    [HttpPost("{id}/deleteDataset")]
    public IActionResult DeleteDataset(string id)
    {
         _datasetService.DeleteDataset(id);
        return Ok();
    }
}