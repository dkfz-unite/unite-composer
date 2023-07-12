using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Composer.Web.Services.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Data.Entities.Specimens.Enums;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

using DrugResource = Unite.Composer.Web.Resources.Domain.Basic.Specimens.DrugScreeningResource;
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
    private readonly SpecimensTsvDownloadService _specimensTsvDownloadService;


    public SpecimenController(
        ISpecimensSearchService specimensSearchService,
        SpecimenDataService specimenDataService,
        SampleDataService sampleDataService,
        DrugScreeningService drugScreeningService,
        GenomicProfileService genomicProfileService,
        SpecimensTsvDownloadService specimensTsvDownloadService)
    {
        _specimensSearchService = specimensSearchService;
        _specimenDataService = specimenDataService;
        _sampleDataService = sampleDataService;
        _drugScreeningService = drugScreeningService;
        _genomicProfileService = genomicProfileService;
        _specimensTsvDownloadService = specimensTsvDownloadService;
    }


    [HttpGet("{id}")]
    public IActionResult Specimen(int id)
    {
        var key = id.ToString();

        var index = _specimensSearchService.Get(key);

        return Json(From(index));
    }


    [HttpPost("{id}/drugs")]
    public IActionResult Drugs(int id)
    {
        var result = _drugScreeningService
            .GetAll(id)
            .Select(model => new DrugResource(model))
            .ToArray();

        return Json(result);
    }


    [HttpGet("{id}/samples")]
    public async Task<IActionResult> Samples(int id)
    {
        var samples = await _sampleDataService.GetSpecimenSamples(id);

        return Json(samples);
    }
    
    [HttpPost("{id}/genes/{sampleId}")]
    public IActionResult Genes(int id, int sampleId, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchGenes(sampleId, searchCriteria);

        return Json(From(sampleId, searchResult));
    }

    [HttpPost("{id}/variants/{sampleId}/{type}")]
    public IActionResult Variants(int id, int sampleId, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchVariants(sampleId, type, searchCriteria);

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
        var index = _specimensSearchService.Get(key);
        var type = index.Tissue != null ? SpecimenType.Tissue
                 : index.Cell != null ? SpecimenType.CellLine
                 : index.Organoid != null ? SpecimenType.Organoid
                 : index.Xenograft != null ? SpecimenType.Xenograft
                 : throw new InvalidOperationException("Unknown specimen type");

        var bytes = await _specimensTsvDownloadService.Download(id, type, model.Data);

        return File(bytes, "application/zip", "data.zip");
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
