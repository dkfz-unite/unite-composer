using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Search.Donors;
using Unite.Composer.Web.Resources.Search.Genes;
using Unite.Composer.Web.Resources.Search.Variants;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Search.Genes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GeneController : Controller
{
    private readonly IGenesSearchService _genesSearchService;


    public GeneController(IGenesSearchService genesSearchService)
    {
        _genesSearchService = genesSearchService;
    }


    [HttpGet("{id}")]
    public GeneResource Get(long id)
    {
        var key = id.ToString();

        var index = _genesSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/donors")]
    public SearchResult<DonorResource> GetDonors(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _genesSearchService.SearchDonors(id, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/variants/{type}")]
    public SearchResult<VariantResource> GetMutations(int id, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _genesSearchService.SearchVariants(id, type, searchCriteria);

        return From(searchResult);
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
