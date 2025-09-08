using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Services.Tsv;
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
    private readonly ImagesDownloadService _tsvDownloadService;


    public ImageController(
        ISearchService<ImageIndex> searchService,
        ImagesDownloadService tsvDownloadService)
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
        Response.ContentType = "application/octet-stream";
        Response.Headers.Append("Content-Disposition", "attachment; filename=data.zip");

        using var stream = Response.BodyWriter.AsStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        await _tsvDownloadService.Download([id], model.Data, archive);

        return new EmptyResult();
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
