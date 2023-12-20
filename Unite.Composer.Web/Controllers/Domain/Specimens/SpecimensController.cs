using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Indices.Entities.Basic.Specimens.Constants;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimensController : DomainController
{
    private readonly ISearchService<SpecimenIndex> _searchService;
    private readonly SpecimensTsvDownloadService _tsvDownloadService;


    public SpecimensController(
        ISearchService<SpecimenIndex> searchService, 
        SpecimensTsvDownloadService tsvDownloadService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpPost("{type}")]
    public async Task<IActionResult> Search(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Type = DetectSpecimenType(type) };

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{type}/stats")]
    public async Task<IActionResult> Stats(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Type = DetectSpecimenType(type) };

        var stats = await _searchService.Stats(criteria);

        return Ok(new SpecimensDataResource(stats, type));
    }

    [HttpPost("{type}/data")]
    public async Task<ActionResult> Data(string type, [FromBody]BulkDownloadModel model)
    {
        var criteria = model.Criteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Type = DetectSpecimenType(type) };

        var stats = await _searchService.Stats(criteria);

        var originalIds = stats.Keys.Cast<int>().ToArray();
        var originalType = Convert(type);
        var bytes = await _tsvDownloadService.Download(originalIds, originalType, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static SearchResult<SpecimenResource> From(SearchResult<SpecimenIndex> searchResult)
    {
        return new SearchResult<SpecimenResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SpecimenResource(index)).ToArray()
        };
    }

    private static Unite.Data.Entities.Specimens.Enums.SpecimenType Convert(string type)
    {
        return type switch
        {
            SpecimenType.Tissue => Unite.Data.Entities.Specimens.Enums.SpecimenType.Tissue,
            SpecimenType.CellLine => Unite.Data.Entities.Specimens.Enums.SpecimenType.CellLine,
            SpecimenType.Organoid => Unite.Data.Entities.Specimens.Enums.SpecimenType.Organoid,
            SpecimenType.Xenograft => Unite.Data.Entities.Specimens.Enums.SpecimenType.Xenograft,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
