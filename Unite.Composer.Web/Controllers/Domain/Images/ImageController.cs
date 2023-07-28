using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Data.Images;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Data.Entities.Images.Enums;

using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;


namespace Unite.Composer.Web.Controllers.Domain.Images;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageController : Controller
{
    private readonly IImagesSearchService _searchService;
    private readonly ImageDataService _dataService;
    private readonly SampleDataService _sampleDataService;
    private readonly GenomicProfileService _genomicProfileService;
    private readonly ImagesTsvDownloadService _imagesTsvDownloadService;


    public ImageController(
        IImagesSearchService searchService,
        ImageDataService dataService,
        SampleDataService sampleDataService,
        GenomicProfileService genomicProfileService,
        ImagesTsvDownloadService imagesTsvDownloadService)
    {
        _searchService = searchService;
        _dataService = dataService;
        _sampleDataService = sampleDataService;
        _genomicProfileService = genomicProfileService;
        _imagesTsvDownloadService = imagesTsvDownloadService;
    }


    [HttpGet("{id}")]
    public IActionResult Image(int id)
    {
        var key = id.ToString();

        var index = _searchService.Get(key);

        return Json(From(index));
    }


    [HttpGet("{id}/samples")]
    public async Task<IActionResult> Samples(int id)
    {
        var samples = await _sampleDataService.GetImageSamples(id);

        return Json(samples);
    }

    [HttpPost("{id}/genes/{sampleId}")]
    public IActionResult Genes(int id, int sampleId, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.SearchGenes(sampleId, searchCriteria);

        return Json(From(sampleId, searchResult));
    }

    [HttpPost("{id}/variants/{sampleId}/{type}")]
    public IActionResult Variants(int id, int sampleId, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.SearchVariants(sampleId, type, searchCriteria);

        return Json(From(sampleId, searchResult));
    }

    [HttpPost("{id}/profile/{sampleId}")]
    public async Task<IActionResult> Profile(int id, int sampleId, [FromBody] GenomicRangesFilterCriteria filterCriteria)
    {
        var profile = await _genomicProfileService.GetProfile(sampleId, filterCriteria);

        return Json(profile);
    }


    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody]SingleDownloadModel model)
    {
        var key = id.ToString();
        var index = _searchService.Get(key);
        var type = index.Mri != null ? ImageType.MRI 
                 : throw new InvalidOperationException("Unknown image type");

        var bytes = await _imagesTsvDownloadService.Download(id, type, model.Data);

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

    private static SearchResult<ImageGeneResource> From(int sampleId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<ImageGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ImageGeneResource(index, sampleId)).ToArray()
        };
    }

    private static SearchResult<VariantResource> From(int sampleId, SearchResult<VariantIndex> searchResult)
    {
        return new SearchResult<VariantResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new VariantResource(index)).ToArray()
        };
    }
}
