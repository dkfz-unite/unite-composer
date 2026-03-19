using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Resources.Domain.Proteins;
using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Proteins;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProteinsController : DomainController
{
    private readonly ISearchService<ProteinIndex> _searchService;
    private readonly TaskStatsService _taskStatsService;


    public ProteinsController(
        ISearchService<ProteinIndex> searchService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
        _taskStatsService = taskStatsService;
    }


    [HttpPost("")]
    public async Task<IActionResult> Proteins([FromBody] SearchCriteria searchCriteria)
    {
        var result = await _searchService.Search(searchCriteria);

        return Ok(From(result));
    }

    [HttpPost("stats")]
    public async Task<IActionResult> Stats([FromBody] SearchCriteria searchCriteria)
    {
        var stats = await _searchService.Stats(searchCriteria);

        return Ok(new ProteinDataResource(stats));
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var status = await _taskStatsService.GetStatus(Unite.Data.Entities.Tasks.Enums.IndexingTaskType.Protein);

        return Ok(status);
    }


    private static SearchResult<ProteinResource> From(SearchResult<ProteinIndex> searchResult)
    {
        return new SearchResult<ProteinResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ProteinResource(index)).ToArray()
        };
    }
}
