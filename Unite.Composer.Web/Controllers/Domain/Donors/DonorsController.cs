using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Donors;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DonorsController : DomainController
{
    private readonly ISearchService<DonorIndex> _searchService;
    private readonly DonorsTsvDownloadService _tsvDownloadService;


    public DonorsController(
        ISearchService<DonorIndex> searchService, 
        DonorsTsvDownloadService tsvDownloadService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpPost("")]
    public async Task<IActionResult> Search([FromBody] SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("stats")]
    public async Task<IActionResult> Stats([FromBody] SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var stats = await _searchService.Stats(criteria);

        return Ok(new DonorsDataResource(stats));
    }

    [HttpPost("data")]
    public async Task<IActionResult> Data([FromBody] BulkDownloadModel model)
    {
        var stats = await _searchService.Stats(model.Criteria);

        var originalIds = stats.Keys.Cast<int>().ToArray();
        var bytes = await _tsvDownloadService.Download(originalIds, model.Data);

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
