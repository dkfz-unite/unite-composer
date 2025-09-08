using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Omics.Ranges;
using Unite.Composer.Data.Omics.Ranges.Models;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Download.Services.Tsv;
using Unite.Composer.Web.Resources.Domain.Genes;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Composer.Web.Models;
using Unite.Indices.Entities.Basic.Specimens.Constants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using CnvIndex = Unite.Indices.Entities.Variants.CnvIndex;
using SvIndex = Unite.Indices.Entities.Variants.SvIndex;

using DrugResource = Unite.Composer.Web.Resources.Domain.Basic.Specimens.DrugScreeningResource;


namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimenController : DomainController
{
    private readonly ISearchService<SpecimenIndex> _specimensSearchService;
    private readonly ISearchService<GeneIndex> _genesSearchService;
    private readonly ISearchService<SmIndex> _smsSearchService;
    private readonly ISearchService<CnvIndex> _cnvsSearchService;
    private readonly ISearchService<SvIndex> _svsSearchService;
    private readonly DrugScreeningService _drugScreeningService;
    private readonly GenomicProfileService _genomicProfileService;
    private readonly SpecimensDownloadService _tsvDownloadService;


    public SpecimenController(
        ISearchService<SpecimenIndex> specimensSearchService,
        ISearchService<GeneIndex> genesSearchService,
        ISearchService<SmIndex> smsSearchService,
        ISearchService<CnvIndex> cnvsSearchService,
        ISearchService<SvIndex> svsSearchService,
        DrugScreeningService drugScreeningService,
        GenomicProfileService genomicProfileService,
        SpecimensDownloadService tsvDownloadService)
    {
        _specimensSearchService = specimensSearchService;
        _genesSearchService = genesSearchService;
        _smsSearchService = smsSearchService;
        _cnvsSearchService = cnvsSearchService;
        _svsSearchService = svsSearchService;
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
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = new ValuesCriteria<int>([id]) };

        var result = await _genesSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/sm")]
    public async Task<IActionResult> Sms(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = new ValuesCriteria<int>([id]) };

        var result = await _smsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/cnv")]
    public async Task<IActionResult> Cnvs(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = new ValuesCriteria<int>([id]) };

        var result = await _cnvsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/sv")]
    public async Task<IActionResult> Svs(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = new ValuesCriteria<int>([id]) };

        var result = await _svsSearchService.Search(criteria);

        return Ok(From(result));
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
        Response.ContentType = "application/octet-stream";
        Response.Headers.Append("Content-Disposition", "attachment; filename=data.zip");

        using var stream = Response.BodyWriter.AsStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        await _tsvDownloadService.Download([id], model.Data, archive);

        return new EmptyResult();
    }
    

    private static SpecimenResource From(SpecimenIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new SpecimenResource(index);
    }

    private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<GeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
        };
    }

    private static SearchResult<SmResource> From(SearchResult<SmIndex> searchResult)
    {
        return new SearchResult<SmResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SmResource(index)).ToArray()
        };
    }

    private static SearchResult<CnvResource> From(SearchResult<CnvIndex> searchResult)
    {
        return new SearchResult<CnvResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new CnvResource(index)).ToArray()
        };
    }

    private static SearchResult<SvResource> From(SearchResult<SvIndex> searchResult)
    {
        return new SearchResult<SvResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SvResource(index)).ToArray()
        };
    }

    private static Unite.Data.Entities.Specimens.Enums.SpecimenType Convert(string type)
    {
        return type switch
        {
            SpecimenType.Material => Unite.Data.Entities.Specimens.Enums.SpecimenType.Material,
            SpecimenType.Line => Unite.Data.Entities.Specimens.Enums.SpecimenType.Line,
            SpecimenType.Organoid => Unite.Data.Entities.Specimens.Enums.SpecimenType.Organoid,
            SpecimenType.Xenograft => Unite.Data.Entities.Specimens.Enums.SpecimenType.Xenograft,
            _ => throw new InvalidOperationException("Unknown specimen type")
        };
    }
}
