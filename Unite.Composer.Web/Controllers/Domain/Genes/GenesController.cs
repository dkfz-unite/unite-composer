using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Genes;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Controllers.Domain.Genes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GenesController : Controller
{
    private readonly IGenesSearchService _searchService;
    private readonly GenesTsvDownloadService _genesTsvDownloadService;


    public GenesController(IGenesSearchService searchService, GenesTsvDownloadService genesTsvDownloadService)
    {
        _searchService = searchService;
        _genesTsvDownloadService = genesTsvDownloadService;
    }


    [HttpPost("")]
    public SearchResult<GeneResource> Genes([FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.Search(searchCriteria);

        return From(searchResult);
    }

    [HttpPost("stats")]
    public GenesDataResource Stats([FromBody] SearchCriteria searchCriteria)
    {
        var stats = _searchService.Stats(searchCriteria);

        return new GenesDataResource(stats);
    }

    [HttpPost("data")]
    public async Task<IActionResult> Data([FromBody] BulkDownloadModel model)
    {
        var stats = _searchService.Stats(model.Criteria);

        var bytes = await _genesTsvDownloadService.Download(stats.Keys.ToArray(), model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<GeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
        };
    }
}
