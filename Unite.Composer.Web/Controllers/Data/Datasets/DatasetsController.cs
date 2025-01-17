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


    [HttpPost]
    public async Task<DatasetModel[]> Load([FromBody] SearchModel model)
    {
        return await _datasetsService.Load(model);
    }

    [HttpDelete]
    public async Task Delete([FromBody] SearchModel model)
    {
        await _datasetsService.Delete(model);
    }
}
