using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Models;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantController : Controller
{
    private readonly IVariantsSearchService _variantsSearchService;
    private readonly MutationDataService _mutationDataService;
    private readonly VariantsTsvDownloadService _variantsTsvDownloadService;


    public VariantController(
        IVariantsSearchService variantsSearchService,
        MutationDataService mutationDataService,
        VariantsTsvDownloadService variantsTsvDownloadService)
    {
        _variantsSearchService = variantsSearchService;
        _mutationDataService = mutationDataService;
        _variantsTsvDownloadService = variantsTsvDownloadService;
    }


    [HttpGet("{id}")]
    public VariantResource Get(string id)
    {
        var key = id;

        var index = _variantsSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/donors")]
    public SearchResult<DonorResource> SearchDonors(string id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _variantsSearchService.SearchDonors(id, searchCriteria);

        return From(searchResult);
    }

    [HttpGet("{id}/translations")]
    public Transcript[] GetTranslations(string id)
    {
        if (id.StartsWith("SSM"))
        {
            var mutationId = long.Parse(id.Substring(3));
            var translations = _mutationDataService.GetTranslations(mutationId);

            return translations;
        }
        else
        {
            return null;
        }
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(string id, [FromBody]SingleDownloadModel model)
    {
        var key = id.ToString();
        var index = _variantsSearchService.Get(key);
        var type = index.Ssm != null ? VariantType.SSM
                 : index.Cnv != null ? VariantType.CNV
                 : index.Sv != null ? VariantType.SV
                 : throw new InvalidOperationException("Unknown variant type");

        var bytes = await _variantsTsvDownloadService.Download(long.Parse(id.Substring(index.Type.Length)), type, model.Data);

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
}
