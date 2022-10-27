using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Search.Donors;
using Unite.Composer.Web.Resources.Search.Variants;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Search.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantController : Controller
{
    private readonly IVariantsSearchService _variantsSearchService;


    public VariantController(IVariantsSearchService mutationsSearchService)
    {
        _variantsSearchService = mutationsSearchService;
    }


    [HttpGet("{id}")]
    public VariantResource Get(string id)
    {
        var key = id;

        var index = _variantsSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/donors")]
    public SearchResult<DonorResource> GetDonors(string id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _variantsSearchService.SearchDonors(id, searchCriteria);

        return From(searchResult);
    }


    private static VariantResource From(VariantIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new VariantResource(index);
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
