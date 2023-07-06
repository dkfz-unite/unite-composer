using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Donors;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Images;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Composer.Web.Resources.Domain.Variants;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Domain.Donors;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DonorController : Controller
{
    private readonly IDonorsSearchService _donorsSearchService;
    private readonly DonorDataService _donorDataService;
    private readonly SampleDataService _sampleDataService;
    private readonly GenomicProfileService _genomicProfileService;

    public DonorController(
        IDonorsSearchService donorsSearchService,
        DonorDataService donorDataService,
        SampleDataService sampleDataService,
        GenomicProfileService genomicProfileService)
    {
        _donorsSearchService = donorsSearchService;
        _donorDataService = donorDataService;
        _sampleDataService = sampleDataService;
        _genomicProfileService = genomicProfileService;
    }


    [HttpGet("{id}")]
    public DonorResource Get(int id)
    {
        var key = id.ToString();

        var index = _donorsSearchService.Get(key);
        
        return From(index);
    }

    [HttpPost("{id}/images/{type}")]
    public SearchResult<ImageResource> SearchImages(int id, ImageType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchImages(id, type, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/specimens")]
    public SearchResult<SpecimenResource> SearchSpecimens(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchSpecimens(id, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/genes/{sampleId}")]
    public SearchResult<DonorGeneResource> SearchGenes(int id, int sampleId, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchGenes(sampleId, searchCriteria);

        return From(sampleId, searchResult);
    }

    [HttpPost("{id}/variants/{sampleId}/{type}")]
    public SearchResult<VariantResource> SearchVariants(int id, int sampleId, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchVariants(sampleId, type, searchCriteria);

        return From(sampleId, searchResult);
    }

    [HttpGet("{id}/samples")]
    public IActionResult GetSamples(int id)
    {
        var samples = _sampleDataService.GetDonorSamples(id);

        return Json(samples);
    }

    [HttpPost("{id}/profile/{sampleId}")]
    public async Task<IActionResult> GetProfile(int id, int sampleId, [FromBody] GenomicRangesFilterCriteria filterCriteria)
    {
        var profile = await _genomicProfileService.GetProfile(sampleId, filterCriteria);

        return Json(profile);
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

    private static SearchResult<DonorGeneResource> From(int sampleId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<DonorGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new DonorGeneResource(index, sampleId)).ToArray()
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
