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
    private readonly DatasetsService _datasetsService;

    public DatasetsController(DatasetsService datasetsService)
    {
        _datasetsService = datasetsService;
    }

    [HttpPost()]
    public async Task<IEnumerable<DatasetModel>> Load([FromBody] DatasetModel dataset)
    {
        return await _datasetsService.Load(dataset);
    }
}