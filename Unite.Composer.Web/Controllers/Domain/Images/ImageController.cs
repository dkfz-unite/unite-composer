using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Data.Images;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Composer.Web.Resources.Domain.Variants;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
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


    public ImageController(
        IImagesSearchService searchService,
        ImageDataService dataService,
        SampleDataService sampleDataService,
        GenomicProfileService genomicProfileService)
    {
        _searchService = searchService;
        _dataService = dataService;
        _sampleDataService = sampleDataService;
        _genomicProfileService = genomicProfileService;
    }


    [HttpGet("{id}")]
    public ImageResource Get(int id)
    {
        var key = id.ToString();

        var index = _searchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/genes/{sampleId}")]
    public SearchResult<ImageGeneResource> GetGenes(int id, int sampleId, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.SearchGenes(sampleId, searchCriteria);

        return From(sampleId, searchResult);
    }

    [HttpPost("{id}/variants/{sampleId}/{type}")]
    public SearchResult<VariantResource> GetVariants(int id, int sampleId, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.SearchVariants(sampleId, type, searchCriteria);

        return From(sampleId, searchResult);
    }

    [HttpGet("{id}/samples")]
    public IActionResult GetSamples(int id)
    {
        var samples = _sampleDataService.GetImageSamples(id);

        return Json(samples);
    }

    [HttpPost("{id}/profile/{sampleId}")]
    public async Task<IActionResult> GetProfile(int id, int sampleId, [FromBody] GenomicRangesFilterCriteria filterCriteria)
    {
        var profile = await _genomicProfileService.GetProfile(sampleId, filterCriteria);

        return Json(profile);
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
