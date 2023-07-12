using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Controllers.Domain.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantsController : Controller
{
    private readonly IVariantsSearchService _searchService;


    public VariantsController(IVariantsSearchService searchService)
    {
        _searchService = searchService;
    }


    [HttpPost("{type}")]
    public SearchResult<VariantResource> Search(VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchContext = new VariantSearchContext(type);

        var searchResult = _searchService.Search(searchCriteria, searchContext);

        return From(searchResult);
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
