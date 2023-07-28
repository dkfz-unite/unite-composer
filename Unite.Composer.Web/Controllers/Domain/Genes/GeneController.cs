using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Models;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Genes;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Domain.Genes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GeneController : Controller
{
    private readonly IGenesSearchService _genesSearchService;
    private readonly GeneDataService _geneDataService;
    private readonly GenesTsvDownloadService _genesTsvDownloadService;


    public GeneController(
        IGenesSearchService genesSearchService, 
        GeneDataService geneDataService, 
        GenesTsvDownloadService genesTsvDownloadService)
    {
        _genesSearchService = genesSearchService;
        _geneDataService = geneDataService;
        _genesTsvDownloadService = genesTsvDownloadService;
    }


    [HttpGet("{id}")]
    public GeneResource Gene(long id)
    {
        var key = id.ToString();

        var index = _genesSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/donors")]
    public SearchResult<DonorResource> Donors(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _genesSearchService.SearchDonors(id, searchCriteria);

        return From(id, searchResult);
    }

    [HttpPost("{id}/variants/{type}")]
    public SearchResult<VariantResource> Variants(int id, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _genesSearchService.SearchVariants(id, type, searchCriteria);

        return From(searchResult);
    }

    [HttpGet("{id}/translations")]
    public Transcript[] Translations(int id)
    {
        var translations = _geneDataService.GetTranslations(id);

        return translations;
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody] SingleDownloadModel model)
    {
        var bytes = await _genesTsvDownloadService.Download(id, model.Data);

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

    private static SearchResult<DonorResource> From(int geneId, SearchResult<DonorIndex> searchResult)
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
