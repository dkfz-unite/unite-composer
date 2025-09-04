using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using System.IO.Compression;
using Unite.Composer.Download.Services.Tsv;

namespace Unite.Composer.Web.Controllers.Domain.Donors;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DonorController : DomainController
{
    private readonly ISearchService<DonorIndex> _donorsSearchService;
    private readonly ISearchService<ImageIndex> _imagesSearchService;
    private readonly ISearchService<SpecimenIndex> _specimensSearchService;
    private readonly DonorsTsvDownloadService _tsvDownloadService;
    private readonly DonorsDownloadService _donorsDownloadService;


    public DonorController(
        ISearchService<DonorIndex> donorsSearchService,
        ISearchService<ImageIndex> imagesSearchService,
        ISearchService<SpecimenIndex> specimensSearchService,
        DonorsTsvDownloadService tsvDownloadService,
        DonorsDownloadService donorsDownloadService)
    {
        _donorsSearchService = donorsSearchService;
        _imagesSearchService = imagesSearchService;
        _specimensSearchService = specimensSearchService;
        _tsvDownloadService = tsvDownloadService;
        _donorsDownloadService = donorsDownloadService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Donor(int id)
    {
        var key = id.ToString();

        var result = await _donorsSearchService.Get(key);

        return Ok(From(result));
    }

    [HttpPost("{id}/images/{type?}")]
    public async Task<IActionResult> Images(int id, string type, [FromBody] SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = new ValuesCriteria<int>([id]) };
        criteria.Image = (criteria.Image ?? new ImagesCriteria()) with { ImageType = new ValuesCriteria<string>(DetectImageType(type)) };

        var result = await _imagesSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/specimens/{type?}")]
    public async Task<IActionResult> Specimens(int id, string type, [FromBody] SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = new ValuesCriteria<int>([id]) };
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { SpecimenType = new ValuesCriteria<string>(DetectSpecimenType(type)) };

        var result = await _specimensSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody] SingleDownloadModel model)
    {
        Response.ContentType = "application/octet-stream";
        Response.Headers.Append("Content-Disposition", "attachment; filename=data.zip");

        var stream = Response.BodyWriter.AsStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        await _donorsDownloadService.Download([id], model.Data, archive);
        await _tsvDownloadService.Download([id], model.Data, archive);

        return new EmptyResult();
    }


    private static DonorResource From(DonorIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new DonorResource(index);
    }

    private static SearchResult<ImageResource> From(SearchResult<ImageIndex> searchResult)
    {
        return new SearchResult<ImageResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ImageResource(index)).ToArray()
        };
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

public class FileCallbackResult : FileResult
{
    private readonly Func<Stream, ActionContext, Task> _callback;

    public FileCallbackResult(string contentType, Func<Stream, ActionContext, Task> callback) 
        : base(contentType)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        await _callback(response.Body, context);
    }
}
