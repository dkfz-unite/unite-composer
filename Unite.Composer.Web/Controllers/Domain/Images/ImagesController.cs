using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Indices.Entities.Images;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Images;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImagesController : DomainController
{
    private readonly ISearchService<ImageIndex> _searchService;
    private readonly ImagesTsvDownloadService _tsvDownloadService;
    private readonly TaskStatsService _taskStatsService;


    public ImagesController(
        ISearchService<ImageIndex> searchService,
        ImagesTsvDownloadService tsvDownloadService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
        _taskStatsService = taskStatsService;
    }


    [HttpPost("{type}")]
    public async Task<IActionResult> Search(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Image = (criteria.Image ?? new ImageCriteria()) with { Type = DetectImageType(type) };

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{type}/stats")]
    public async Task<IActionResult> Stats(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Image = (criteria.Image ?? new ImageCriteria()) with { Type = DetectImageType(type) };

        var stats = await _searchService.Stats(criteria);

        return Ok(new ImagesDataResource(stats));
    }

    [HttpPost("{type}/data")]
    public async Task<IActionResult> Data(string type, [FromBody] BulkDownloadModel model)
    {
        var criteria = model.Criteria ?? new SearchCriteria();
        criteria.Image = (criteria.Image ?? new ImageCriteria()) with { Type = DetectImageType(type) };

        var stats = await _searchService.Stats(criteria);

        var originalIds = stats.Keys.Cast<int>().ToArray();
        var originalType = ConvertImageType(type);
        var bytes = await _tsvDownloadService.Download(originalIds, originalType, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }

    [HttpGet("{type}/status")]
    public async Task<IActionResult> Status(string type)
    {
        var status = await _taskStatsService.GetStatus(Unite.Data.Entities.Tasks.Enums.IndexingTaskType.Image);

        return Ok(status);
    }


    private static SearchResult<ImageResource> From(SearchResult<ImageIndex> searchResult)
    {
        return new SearchResult<ImageResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ImageResource(index)).ToArray()
        };
    }
}
