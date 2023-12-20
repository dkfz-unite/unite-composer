using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Composer.Web.Models;
using Unite.Indices.Entities.Basic.Specimens.Constants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

using DrugResource = Unite.Composer.Web.Resources.Domain.Basic.Specimens.DrugScreeningResource;
using VariantResource = Unite.Composer.Web.Resources.Domain.Variants.VariantResource;


namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimenController : DomainController
{
    private readonly ISearchService<SpecimenIndex> _specimensSearchService;
    private readonly ISearchService<GeneIndex> _genesSearchService;
    private readonly ISearchService<VariantIndex> _variantsSearchService;
    private readonly DrugScreeningService _drugScreeningService;
    private readonly GenomicProfileService _genomicProfileService;
    private readonly SpecimensTsvDownloadService _tsvDownloadService;


    public SpecimenController(
        ISearchService<SpecimenIndex> specimensSearchService,
        ISearchService<GeneIndex> genesSearchService,
        ISearchService<VariantIndex> variantsSearchService,
        DrugScreeningService drugScreeningService,
        GenomicProfileService genomicProfileService,
        SpecimensTsvDownloadService tsvDownloadService)
    {
        _specimensSearchService = specimensSearchService;
        _genesSearchService = genesSearchService;
        _variantsSearchService = variantsSearchService;
        _drugScreeningService = drugScreeningService;
        _genomicProfileService = genomicProfileService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Specimen(int id)
    {
        var key = id.ToString();

        var result = await _specimensSearchService.Get(key);

        return Ok(From(result));
    }

    [HttpPost("{id}/drugs")]
    public async Task<IActionResult> Drugs(int id)
    {
        var result = await _drugScreeningService.GetAll(id);

        var resource = result
            .Select(model => new DrugResource(model))
            .ToArray();

        return await OkAsync(resource);
    }

    [HttpPost("{id}/genes")]
    public async Task<IActionResult> Genes(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Id = [id] };

        var result = await _genesSearchService.Search(criteria);

        return Ok(From(id, result));
    }

    [HttpPost("{id}/variants/{type}")]
    public async Task<IActionResult> Variants(int id, string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Id = [id] };
        criteria.Variant = (criteria.Variant ?? new VariantCriteria()) with { Type = DetectVariantType(type) };

        var result = await _variantsSearchService.Search(criteria);

        return Ok(From(id, result));
    }

    [HttpPost("{id}/profile")]
    public async Task<IActionResult> Profile(int id, [FromBody]GenomicRangesFilterCriteria filterCriteria)
    {
        var profile = await _genomicProfileService.GetProfile(id, filterCriteria);

        return Ok(profile);
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody]SingleDownloadModel model)
    {
        var key = id.ToString();
        var index = await _specimensSearchService.Get(key);

        var originalType = Convert(index.Type);
        var bytes = await _tsvDownloadService.Download(id, originalType, model.Data);

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

    private static SearchResult<SpecimenGeneResource> From(int specimenId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<SpecimenGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SpecimenGeneResource(index, specimenId)).ToArray()
        };
    }

    private static SearchResult<VariantResource> From(int specimenId, SearchResult<VariantIndex> searchResult)
    {
        return new SearchResult<VariantResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new VariantResource(index)).ToArray()
        };
    }

    private static Unite.Data.Entities.Specimens.Enums.SpecimenType Convert(string type)
    {
        return type switch
        {
            SpecimenType.Tissue => Unite.Data.Entities.Specimens.Enums.SpecimenType.Tissue,
            SpecimenType.CellLine => Unite.Data.Entities.Specimens.Enums.SpecimenType.CellLine,
            SpecimenType.Organoid => Unite.Data.Entities.Specimens.Enums.SpecimenType.Organoid,
            SpecimenType.Xenograft => Unite.Data.Entities.Specimens.Enums.SpecimenType.Xenograft,
            _ => throw new InvalidOperationException("Unknown specimen type")
        };
    }
}
