using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Variants;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SmsController : DomainController
{
    private readonly ISearchService<SmIndex> _searchService;
    private readonly TaskStatsService _taskStatsService;


    public SmsController(
        ISearchService<SmIndex> searchService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
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

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var status = await _taskStatsService.GetStatus(IndexingTaskType.SM);

        return Ok(status);
    }

    
    private static SearchResult<SmResource> From(SearchResult<SmIndex> searchResult)
    {
        return new SearchResult<SmResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SmResource(index)).ToArray()
        };
    }
}
