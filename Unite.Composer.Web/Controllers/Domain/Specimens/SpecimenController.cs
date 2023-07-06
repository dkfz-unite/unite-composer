using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Domain.Specimens;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

using DrugScreeningResource = Unite.Composer.Web.Resources.Domain.Basic.Specimens.DrugScreeningResource;
using VariantResource = Unite.Composer.Web.Resources.Domain.Variants.VariantResource;


namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimenController : Controller
{
    private readonly ISpecimensSearchService _specimensSearchService;
    private readonly SpecimenDataService _specimenDataService;
    private readonly SampleDataService _sampleDataService;
    private readonly DrugScreeningService _drugScreeningService;
    private readonly GenomicProfileService _genomicProfileService;


    public SpecimenController(
        ISpecimensSearchService specimensSearchService,
        SpecimenDataService specimenDataService,
        SampleDataService sampleDataService,
        DrugScreeningService drugScreeningService,
        GenomicProfileService genomicProfileService)
    {
        _specimensSearchService = specimensSearchService;
        _specimenDataService = specimenDataService;
        _sampleDataService = sampleDataService;
        _drugScreeningService = drugScreeningService;
        _genomicProfileService = genomicProfileService;
    }


    [HttpGet("{id}")]
    public SpecimenResource Get(int id)
    {
        var key = id.ToString();

        var index = _specimensSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/genes/{sampleId}")]
    public SearchResult<SpecimenGeneResource> GetGenes(int id, int sampleId, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchGenes(sampleId, searchCriteria);

        return From(sampleId, searchResult);
    }

    [HttpPost("{id}/variants/{sampleId}/{type}")]
    public SearchResult<VariantResource> GetVariants(int id, int sampleId, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchVariants(sampleId, type, searchCriteria);

        return From(sampleId, searchResult);
    }

    [HttpGet("{id}/samples")]
    public IActionResult GetSamples(int id)
    {
        var samples = _sampleDataService.GetSpecimenSamples(id).ToArray();

        return Json(samples);
    }

    [HttpPost("{id}/profile/{sampleId}")]
    public async Task<IActionResult> GetProfile(int id, int sampleId, [FromBody] GenomicRangesFilterCriteria filterCriteria)
    {
        var profile = await _genomicProfileService.GetProfile(sampleId, filterCriteria);

        return Json(profile);
    }

    [HttpPost("{id}/drugs")]
    public DrugScreeningResource[] GetDrugsScreeningData(int id)
    {
        var result = _drugScreeningService
            .GetAll(id)
            .Select(model => new DrugScreeningResource(model))
            .ToArray();

        return result;
    }


    private static SpecimenResource From(SpecimenIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new SpecimenResource(index);
    }

    private static SearchResult<SpecimenGeneResource> From(int sampleId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<SpecimenGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SpecimenGeneResource(index, sampleId)).ToArray()
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
