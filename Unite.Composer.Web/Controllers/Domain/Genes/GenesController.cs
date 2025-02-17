using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Genes;
using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Genes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GenesController : DomainController
{
    private readonly ISearchService<GeneIndex> _searchService;
    private readonly GenesTsvDownloadService _tsvDownloadService;
    private readonly TaskStatsService _taskStatsService;


    public GenesController(
        ISearchService<GeneIndex> searchService, 
        GenesTsvDownloadService tsvDownloadService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
        _taskStatsService = taskStatsService;
    }


    [HttpPost("")]
    public async Task<IActionResult> Genes([FromBody] SearchCriteria searchCriteria)
    {
        var result = await _searchService.Search(searchCriteria);

        return Ok(From(result));
    }

    [HttpPost("stats")]
    public async Task<IActionResult> Stats([FromBody] SearchCriteria searchCriteria)
    {
        var stats = await _searchService.Stats(searchCriteria);

        return Ok(new GeneDataResource(stats));
    }

    [HttpPost("data")]
    public async Task<IActionResult> Data([FromBody] BulkDownloadModel model)
    {
        var stats = await _searchService.Stats(model.Criteria);

        var originalIds = stats.Keys.Cast<int>().ToArray();
        var bytes = await _tsvDownloadService.Download(originalIds, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var status = await _taskStatsService.GetStatus(Unite.Data.Entities.Tasks.Enums.IndexingTaskType.Gene);

        return Ok(status);
    }


    private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<GeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
        };
    }
}
