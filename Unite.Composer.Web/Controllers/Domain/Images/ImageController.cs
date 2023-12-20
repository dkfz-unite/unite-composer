using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Indices.Search.Services;

using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;

namespace Unite.Composer.Web.Controllers.Domain.Images;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageController : DomainController
{
    private readonly ISearchService<ImageIndex> _searchService;
    private readonly ImagesTsvDownloadService _tsvDownloadService;


    public ImageController(
        ISearchService<ImageIndex> searchService,
        ImagesTsvDownloadService tsvDownloadService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Image(int id)
    {
        var key = id.ToString();

        var result = await _searchService.Get(key);

        return Ok(From(result));
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody]SingleDownloadModel model)
    {
        var key = id.ToString();
        
        var index = await _searchService.Get(key);

        var originalType = ConvertImageType(index.Type);
        var bytes = await _tsvDownloadService.Download(id, originalType, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static ImageResource From(ImageIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new ImageResource(index);
    }
}
