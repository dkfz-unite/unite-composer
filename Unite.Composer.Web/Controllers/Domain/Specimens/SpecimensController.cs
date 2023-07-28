using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Data.Entities.Specimens.Enums;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimensController : Controller
{
    private readonly ISpecimensSearchService _searchService;
    private readonly SpecimensTsvDownloadService _tsvDownloadService;


    public SpecimensController(
        ISpecimensSearchService searchService, 
        SpecimensTsvDownloadService tsvDownloadService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpPost("{type}")]
    public SearchResult<SpecimenResource> Search(SpecimenType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new SpecimenSearchContext(type);

        var searchResult = _searchService.Search(searchCriteria, searchContext);

        return From(searchResult);
    }

    [HttpPost("{type}/stats")]
    public SpecimensDataResource Stats(SpecimenType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new SpecimenSearchContext(type);

        var availableData = _searchService.Stats(searchCriteria, searchContext);

        return new SpecimensDataResource(availableData);
    }

    [HttpPost("{type}/data")]
    public async Task<ActionResult> Data(SpecimenType type, BulkDownloadModel model)
    {
        var context = new SpecimenSearchContext(type);
        var stats = _searchService.Stats(model.Criteria, context);
        var bytes = await _tsvDownloadService.Download(stats.Keys.ToArray(), type, model.Data);

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
}
