using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Composer.Web.Services.Download.Tsv;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimensController : Controller
{
    private readonly ISpecimensSearchService _searchService;
    private readonly SpecimensTsvDownloadService _specimensTsvDownloadService;


    public SpecimensController(
        ISpecimensSearchService searchService, 
        SpecimensTsvDownloadService specimensTsvDownloadService)
    {
        _searchService = searchService;
        _specimensTsvDownloadService = specimensTsvDownloadService;
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
    public async Task<ActionResult> Data(SpecimenType type, DownloadDataModel model)
    {
        var context = new SpecimenSearchContext(type);
        var stats = _searchService.Stats(model.Criteria, context);
        var bytes = await _specimensTsvDownloadService.Download(type, stats.Keys.ToArray(), model.Data);

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
