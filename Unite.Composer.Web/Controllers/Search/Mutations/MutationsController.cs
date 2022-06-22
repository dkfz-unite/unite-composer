using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Mutations;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Controllers.Search.Mutations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MutationsController : Controller
{
    private readonly IMutationsSearchService _searchService;


    public MutationsController(IMutationsSearchService searchService)
    {
        _searchService = searchService;
    }


    [HttpPost("")]
    public SearchResult<MutationResource> Search([FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.Search(searchCriteria);

        return From(searchResult);
    }


    private static SearchResult<MutationResource> From(SearchResult<MutationIndex> searchResult)
    {
        return new SearchResult<MutationResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new MutationResource(index)).ToArray()
        };
    }
}
