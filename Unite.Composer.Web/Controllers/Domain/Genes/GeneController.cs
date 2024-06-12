using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Data.Genome;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Genes;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Domain.Genes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GeneController : DomainController
{
    private readonly ISearchService<DonorIndex> _donorsSearchService;
    private readonly ISearchService<GeneIndex> _genesSearchService;
    private readonly ISearchService<VariantIndex> _variantsSearchService;
    private readonly GeneDataService _dataService;
    private readonly GenesTsvDownloadService _tsvDownloadService;


    public GeneController(
        ISearchService<DonorIndex> donorsSearchService,
        ISearchService<GeneIndex> genesSearchService,
        ISearchService<VariantIndex> variantsSearchService,
        GeneDataService dataService, 
        GenesTsvDownloadService tsvDownloadService)
    {
        _donorsSearchService = donorsSearchService;
        _genesSearchService = genesSearchService;
        _variantsSearchService = variantsSearchService;
        _dataService = dataService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Gene(int id)
    {
        var key = id.ToString();

        var result = await _genesSearchService.Get(key);

        return Ok(From(result));
    }

    [HttpPost("{id}/donors")]
    public async Task<IActionResult> Donors(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Gene = (criteria.Gene ?? new GeneCriteria()) with { Id = [id] };

        var result = await _donorsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{id}/variants/{type?}")]
    public async Task<IActionResult> Variants(int id, string type, [FromBody] SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Gene = (criteria.Gene ?? new GeneCriteria()) with { Id = [id] };
        criteria.Variant = (criteria.Variant ?? new VariantCriteria()) with { Type = DetectVariantType(type) };

        var result = await _variantsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpGet("{id}/translations")]
    public async Task<IActionResult> Translations(int id)
    {
        var translations = await _dataService.GetTranslations(id);

        return Ok(translations);
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody] SingleDownloadModel model)
    {
        var bytes = await _tsvDownloadService.Download(id, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static GeneResource From(GeneIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new GeneResource(index);
    }

    private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
    {
        return new SearchResult<DonorResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
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
