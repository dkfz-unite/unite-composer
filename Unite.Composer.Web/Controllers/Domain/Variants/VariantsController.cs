using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantsController : Controller
{
    private readonly IVariantsSearchService _searchService;
    private readonly VariantsTsvDownloadService _tsvDownloadService;


    public VariantsController(IVariantsSearchService searchService, VariantsTsvDownloadService tsvDownloadService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpPost("{type}")]
    public SearchResult<VariantResource> Search(VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new VariantSearchContext(type);

        var searchResult = _searchService.Search(searchCriteria, searchContext);

        return From(searchResult);
    }

    [HttpPost("{type}/stats")]
    public VariantsDataResource Stats(VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new VariantSearchContext(type);

        var availableData = _searchService.Stats(searchCriteria, searchContext);

        return new VariantsDataResource(availableData, type);
    }

    [HttpPost("{type}/data")]
    public async Task<ActionResult> Data(VariantType type, BulkDownloadModel model)
    {
        var context = new VariantSearchContext(type);
        var stats = _searchService.Stats(model.Criteria, context);
        var bytes = await _tsvDownloadService.Download(stats.Keys.ToArray(), type, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static SearchResult<VariantResource> From(SearchResult<VariantIndex> searchResult)
    {
        return new SearchResult<VariantResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new VariantResource(index)).ToArray()
        };
    }
}
