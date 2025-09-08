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
public class CnvsController : Controller
{
    private readonly ISearchService<CnvIndex> _searchService;
    private readonly TaskStatsService _taskStatsService;


    public CnvsController(
        ISearchService<CnvIndex> searchService,
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
        var status = await _taskStatsService.GetStatus(IndexingTaskType.CNV);

        return Ok(status);
    }

    
    private static SearchResult<CnvResource> From(SearchResult<CnvIndex> searchResult)
    {
        return new SearchResult<CnvResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new CnvResource(index)).ToArray()
        };
    }
}
