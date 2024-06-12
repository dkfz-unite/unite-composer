using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Indices.Entities.Basic.Genome.Dna.Constants;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantsController : DomainController
{
    private readonly ISearchService<VariantIndex> _searchService;
    private readonly VariantsTsvDownloadService _tsvDownloadService;
    private readonly TaskStatsService _taskStatsService;


    public VariantsController(
        ISearchService<VariantIndex> searchService, 
        VariantsTsvDownloadService tsvDownloadService,
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
        criteria.Variant = (criteria.Variant ?? new VariantCriteria()) with { Type = DetectVariantType(type) };

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{type}/stats")]
    public async Task<IActionResult> Stats(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Variant = (criteria.Variant ?? new VariantCriteria()) with { Type = DetectVariantType(type) };

        var data = await _searchService.Stats(criteria);

        return Ok(new VariantsDataResource(data, type));
    }

    [HttpPost("{type}/data")]
    public async Task<ActionResult> Data(string type, BulkDownloadModel model)
    {
        var criteria = model.Criteria ?? new SearchCriteria();
        criteria.Variant = (criteria.Variant ?? new VariantCriteria()) with { Type = DetectVariantType(type) };

        var stats = await _searchService.Stats(criteria);

        var originalIds = Convert(type, stats.Keys.Cast<string>());
        var originalType = Convert(type);
        var bytes = await _tsvDownloadService.Download(originalIds, originalType, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }

    [HttpGet("{type}/status")]
    public async Task<IActionResult> Status(string type)
    {
        var taskType = ConvertTaskType(type);

        var status = await _taskStatsService.GetStatus(taskType);

        return Ok(status);
    }


    private static SearchResult<VariantResource> From(SearchResult<VariantIndex> searchResult)
    {
        return new SearchResult<VariantResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new VariantResource(index)).ToArray()
        };
    }

    private static int[] Convert(string type, IEnumerable<string> ids)
    {
        return ids.Select(id => int.Parse(id[type.Length..])).ToArray();
    }

    private static Unite.Data.Entities.Genome.Analysis.Dna.Enums.VariantType Convert(string type)
    {
        return type switch
        {
            VariantType.SSM => Unite.Data.Entities.Genome.Analysis.Dna.Enums.VariantType.SSM,
            VariantType.CNV => Unite.Data.Entities.Genome.Analysis.Dna.Enums.VariantType.CNV,
            VariantType.SV => Unite.Data.Entities.Genome.Analysis.Dna.Enums.VariantType.SV,
            _ => throw new InvalidOperationException("Unknown variant type")
        };
    }

    private static Unite.Data.Entities.Tasks.Enums.IndexingTaskType ConvertTaskType(string type)
    {
        return type switch
        {
            VariantType.SSM => Unite.Data.Entities.Tasks.Enums.IndexingTaskType.SSM,
            VariantType.CNV => Unite.Data.Entities.Tasks.Enums.IndexingTaskType.CNV,
            VariantType.SV => Unite.Data.Entities.Tasks.Enums.IndexingTaskType.SV,
            _ => throw new InvalidOperationException("Unknown variant type")
        };
    }
}
