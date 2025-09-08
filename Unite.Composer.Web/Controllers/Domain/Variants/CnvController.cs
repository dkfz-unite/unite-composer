using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Donors;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Omics.Analysis.Dna.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.CnvIndex;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CnvController : DomainController
{
    private readonly ISearchService<DonorIndex> _donorsSearchService;
    private readonly ISearchService<VariantIndex> _variantsSearchService;


    public CnvController(
        ISearchService<DonorIndex> donorsSearchService,
        ISearchService<VariantIndex> variantsSearchService)
    {
        _donorsSearchService = donorsSearchService;
        _variantsSearchService = variantsSearchService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var key = id;

        var result = await _variantsSearchService.Get(key);

        // result.Similars

        return Ok(From(result));
    }

    [HttpPost("{id}/donors")]
    public async Task<IActionResult> SearchDonors(int id, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        criteria.Cnv = (criteria.Cnv ?? new CnvCriteria()) with { Id = new ValuesCriteria<int>([id]) };

        var result = await _donorsSearchService.Search(criteria);

        return Ok(From(result));
    }


    private static CnvResource From(VariantIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new CnvResource(index, true);
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
