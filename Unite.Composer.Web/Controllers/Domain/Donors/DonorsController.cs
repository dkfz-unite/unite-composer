using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Services.Download.Tsv;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers.Domain.Donors;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DonorsController : Controller
{
    private readonly IDonorsSearchService _donorsSearchService;
    private readonly DonorsTsvDownloadService _donorsTsvDownloadService;


    public DonorsController(
        IDonorsSearchService donorsSearchService, 
        DonorsTsvDownloadService donorsTsvDownloadService)
    {
        _donorsSearchService = donorsSearchService;
        _donorsTsvDownloadService = donorsTsvDownloadService;
    }


    [HttpPost("")]
    public SearchResult<DonorResource> Search([FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.Search(searchCriteria);

        return From(searchResult);
    }

    [HttpPost("stats")]
    public DonorsDataResource Stats([FromBody] SearchCriteria searchCriteria)
    {
        var stats = _donorsSearchService.Stats(searchCriteria);

        return new DonorsDataResource(stats);
    }

    [HttpPost("data")]
    public async Task<IActionResult> Data([FromBody] DownloadDataModel model)
    {
        var stats = _donorsSearchService.Stats(model.Criteria);

        var bytes = await _donorsTsvDownloadService.Download(stats.Keys.ToArray(), model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
    {
        return new SearchResult<DonorResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
        };
    }
}
