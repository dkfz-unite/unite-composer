using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Composer.Web.Services.Download.Tsv;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Controllers.Domain.Images;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImagesController : Controller
{
    private readonly IImagesSearchService _searchService;
    private readonly ImagesTsvDownloadService _imagesTsvDownloadService;


    public ImagesController(
        IImagesSearchService searchService,
        ImagesTsvDownloadService imagesTsvDownloadService)
    {
        _searchService = searchService;
        _imagesTsvDownloadService = imagesTsvDownloadService;
    }


    [HttpPost("{type}")]
    public SearchResult<ImageResource> Search(ImageType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new ImageSearchContext(type);

        var searchResult = _searchService.Search(searchCriteria, searchContext);

        return From(searchResult);
    }

    [HttpPost("{type}/stats")]
    public ImagesDataResource Stats(ImageType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new ImageSearchContext(type);

        var availableData = _searchService.Stats(searchCriteria, searchContext);

        return new ImagesDataResource(availableData);
    }

    [HttpPost("{type}/data")]
    public async Task<IActionResult> Data(ImageType type, [FromBody] DownloadDataModel model)
    {
        var context = new ImageSearchContext(type);
        var stats = _searchService.Stats(model.Criteria, context);
        var bytes = await _imagesTsvDownloadService.Download(type, stats.Keys.ToArray(), model.Data);

        return File(bytes, "application/zip", "data.zip");
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
