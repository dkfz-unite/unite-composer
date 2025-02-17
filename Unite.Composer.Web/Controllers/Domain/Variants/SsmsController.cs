using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Genome.Analysis.Dna.Enums;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SsmsController : DomainController
{
    private readonly ISearchService<SsmIndex> _searchService;
    private readonly VariantsTsvDownloadService _tsvDownloadService;
    private readonly TaskStatsService _taskStatsService;


    public SsmsController(
        ISearchService<SsmIndex> searchService,
        VariantsTsvDownloadService tsvDownloadService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
        _taskStatsService = taskStatsService;
    }


    [HttpPost("")]
    public async Task<IActionResult> Search(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("stats")]
    public async Task<IActionResult> Stats([FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var data = await _searchService.Stats(criteria);

        return Ok(new VariantDataResource(data));
    }

    [HttpPost("data")]
    public async Task<ActionResult> Data(BulkDownloadModel model)
    {
        var criteria = model.Criteria ?? new SearchCriteria();

        var stats = await _searchService.Stats(criteria);

        var originalIds = stats.Keys.Cast<int>();
        var bytes = await _tsvDownloadService.Download(originalIds, VariantType.SSM, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var status = await _taskStatsService.GetStatus(IndexingTaskType.SSM);

        return Ok(status);
    }

    
    private static SearchResult<SsmResource> From(SearchResult<SsmIndex> searchResult)
    {
        return new SearchResult<SsmResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SsmResource(index)).ToArray()
        };
    }
}
