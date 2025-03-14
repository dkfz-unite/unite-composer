using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Resources.Domain.Basic;
using Unite.Composer.Web.Resources.Domain.Projects;
using Unite.Indices.Entities.Projects;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Projects;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectsController : DomainController
{
    private readonly ISearchService<ProjectIndex> _searchService;
    // private readonly ProjectsTsvDownloadService _tsvDownloadService;
    private readonly TaskStatsService _taskStatsService;


    public ProjectsController(
        ISearchService<ProjectIndex> searchService,
        // ProjectsTsvDownloadService tsvDownloadService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
        _taskStatsService = taskStatsService;
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

        return Ok(new DataResource(stats));
    }

    // [HttpPost("data")]
    // public async Task<IActionResult> Data([FromBody] BulkDownloadModel model)
    // {
    //     var stats = await _searchService.Stats(model.Criteria);

    //     var originalIds = stats.Keys.Cast<int>().ToArray();
    //     var bytes = await _tsvDownloadService.Download(originalIds, model.Data);

    //     return File(bytes, "application/zip", "data.zip");
    // }

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var status = await _taskStatsService.GetStatus(Unite.Data.Entities.Tasks.Enums.IndexingTaskType.Donor);

        return Ok(status);
    }


    private static SearchResult<ProjectResource> From(SearchResult<ProjectIndex> searchResult)
    {
        return new SearchResult<ProjectResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ProjectResource(index)).ToArray()
        };
    }
}
