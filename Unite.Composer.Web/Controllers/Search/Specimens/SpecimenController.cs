using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Search.Specimens;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

using DrugScreeningResource = Unite.Composer.Web.Resources.Search.Basic.Specimens.DrugScreeningResource;
using VariantResource = Unite.Composer.Web.Resources.Search.Variants.VariantResource;

namespace Unite.Composer.Web.Controllers.Search.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimenController : Controller
{
    private readonly ISpecimensSearchService _specimensSearchService;
    private readonly SpecimenDataService _specimenDataService;
    private readonly DrugScreeningService _drugScreeningService;
    private readonly GenomicProfileService _genomicProfileService;


    public SpecimenController(
        ISpecimensSearchService specimensSearchService,
        SpecimenDataService specimenDataService,
        DrugScreeningService drugScreeningService,
        GenomicProfileService genomicProfileService)
    {
        _specimensSearchService = specimensSearchService;
        _specimenDataService = specimenDataService;
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

    [HttpPost("{id}/genes")]
    public SearchResult<SpecimenGeneResource> GetGenes(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchGenes(id, searchCriteria);

        return From(id, searchResult);
    }

    [HttpPost("{id}/variants/{type}")]
    public SearchResult<VariantResource> GetVariants(int id, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchVariants(id, type, searchCriteria);

        return From(searchResult);
    }

    [HttpGet("{id}/samples")]
    public IActionResult GetSamples(int id)
    {
        var samples = _specimenDataService.GetAnalysedSamples(id).ToArray();

        return Json(samples);
    }

    [HttpPost("{id}/profile/{sampleId}")]
    public IActionResult GetProfile(int id, int sampleId, [FromBody] GenomicRangesFilterCriteria filterCriteria)
    {
        var profile = _genomicProfileService.GetProfile(sampleId, filterCriteria);

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

    private static SearchResult<SpecimenGeneResource> From(int specimenId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<SpecimenGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SpecimenGeneResource(index, specimenId)).ToArray()
        };
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
