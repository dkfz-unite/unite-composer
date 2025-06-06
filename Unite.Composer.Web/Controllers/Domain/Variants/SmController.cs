using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Omics;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Omics.Analysis.Dna.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.SmIndex;

namespace Unite.Composer.Web.Controllers.Domain.Variants;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SmController : DomainController
{
    private readonly ISearchService<DonorIndex> _donorsSearchService;
    private readonly ISearchService<VariantIndex> _variantsSearchService;
    private readonly SmDataService _variantsDataService;
    private readonly VariantsTsvDownloadService _tsvDownloadService;


    public SmController(
        ISearchService<DonorIndex> donorsSearchService,
        ISearchService<VariantIndex> variantsSearchService,
        SmDataService smDataService,
        VariantsTsvDownloadService tsvDownloadService)
    {
        _donorsSearchService = donorsSearchService;
        _variantsSearchService = variantsSearchService;
        _variantsDataService = smDataService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var key = id;

        var result = await _variantsSearchService.Get(key);

        return Ok(From(result));
    }

    [HttpPost("{id}/donors")]
    public async Task<IActionResult> SearchDonors(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Sm = (criteria.Sm ?? new SmCriteria()) with { Id = [id] };

        var result = await _donorsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpGet("{id}/translations")]
    public async Task<IActionResult> GetTranslations(int id)
    {
        var translations = await _variantsDataService.GetTranslations(id);

        return Ok(translations);
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody]SingleDownloadModel model)
    {
        var bytes = await _tsvDownloadService.Download(id, VariantType.SM, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static SmResource From(VariantIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new SmResource(index, true);
    }

    private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
    {
        return new SearchResult<DonorResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
        };
    }
}
