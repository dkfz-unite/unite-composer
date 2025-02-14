using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Download.Tsv;
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
using SsmIndex = Unite.Indices.Entities.Variants.SsmIndex;
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
    private readonly ISearchService<SsmIndex> _ssmsSearchService;
    private readonly ISearchService<CnvIndex> _cnvsSearchService;
    private readonly ISearchService<SvIndex> _svsSearchService;
    private readonly DrugScreeningService _drugScreeningService;
    private readonly GenomicProfileService _genomicProfileService;
    private readonly SpecimensTsvDownloadService _tsvDownloadService;


    public SpecimenController(
        ISearchService<SpecimenIndex> specimensSearchService,
        ISearchService<GeneIndex> genesSearchService,
        ISearchService<SsmIndex> ssmsSearchService,
        ISearchService<CnvIndex> cnvsSearchService,
        ISearchService<SvIndex> svsSearchService,
        DrugScreeningService drugScreeningService,
        GenomicProfileService genomicProfileService,
        SpecimensTsvDownloadService tsvDownloadService)
    {
        _specimensSearchService = specimensSearchService;
        _genesSearchService = genesSearchService;
        _ssmsSearchService = ssmsSearchService;
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
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = [id] };

        var result = await _genesSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/ssm")]
    public async Task<IActionResult> Ssms(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = [id] };

        var result = await _ssmsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/cnv")]
    public async Task<IActionResult> Cnvs(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = [id] };

        var result = await _cnvsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/sv")]
    public async Task<IActionResult> Svs(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Specimen = (criteria.Specimen ?? new SpecimensCriteria()) with { Id = [id] };

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

    private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<GeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
        };
    }

    private static SearchResult<SsmResource> From(SearchResult<SsmIndex> searchResult)
    {
        return new SearchResult<SsmResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SsmResource(index)).ToArray()
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
