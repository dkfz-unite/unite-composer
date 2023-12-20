using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Indices.Entities.Basic.Genome.Variants.Constants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantController : DomainController
{
    private readonly ISearchService<DonorIndex> _donorsSearchService;
    private readonly ISearchService<VariantIndex> _variantsSearchService;
    private readonly SsmDataService _ssmDataService;
    private readonly VariantsTsvDownloadService _tsvDownloadService;


    public VariantController(
        ISearchService<DonorIndex> donorsSearchService,
        ISearchService<VariantIndex> variantsSearchService,
        SsmDataService ssmDataService,
        VariantsTsvDownloadService tsvDownloadService)
    {
        _donorsSearchService = donorsSearchService;
        _variantsSearchService = variantsSearchService;
        _ssmDataService = ssmDataService;
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
    public async Task<IActionResult> SearchDonors(string id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Variant = (criteria.Variant ?? new VariantCriteria()) with { Id = [id] };

        var result = await _donorsSearchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpGet("{id}/translations")]
    public async Task<IActionResult> GetTranslations(string id)
    {
        if (id.StartsWith(VariantType.SSM))
        {
            var variantId = long.Parse(id.Substring(VariantType.SSM.Length));
            var translations = await _ssmDataService.GetTranslations(variantId);

            return Ok(translations);
        }
        
        return null;
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(string id, [FromBody]SingleDownloadModel model)
    {
        var key = id.ToString();
        var index = await _variantsSearchService.Get(key);

        var originalIds = long.Parse(id.Substring(index.Type.Length));
        var originalType = Convert(index.Type);
        var bytes = await _tsvDownloadService.Download(originalIds, originalType, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static VariantResource From(VariantIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new VariantResource(index, true);
    }

    private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
    {
        return new SearchResult<DonorResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
        };
    }

    private static Unite.Data.Entities.Genome.Variants.Enums.VariantType Convert(string type)
    {
        return type switch
        {
            VariantType.SSM => Unite.Data.Entities.Genome.Variants.Enums.VariantType.SSM,
            VariantType.CNV => Unite.Data.Entities.Genome.Variants.Enums.VariantType.CNV,
            VariantType.SV => Unite.Data.Entities.Genome.Variants.Enums.VariantType.SV,
            _ => throw new InvalidOperationException("Unknown variant type")
        };
    }
}
