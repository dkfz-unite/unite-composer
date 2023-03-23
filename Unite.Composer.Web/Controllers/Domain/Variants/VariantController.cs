using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Models;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Variants;

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


    public VariantController(
        IVariantsSearchService variantsSearchService,
        MutationDataService mutationDataService)
    {
        _variantsSearchService = variantsSearchService;
        _mutationDataService = mutationDataService;
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
