using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Indices.Entities.CnvProfiles;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Composer.Web.Resources.Domain.Variants;
using Unite.Indices.Search.Engine.Queries;


namespace Unite.Composer.Web.Controllers.Domain.Variants;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CnvProfilesController : Controller
{
    private readonly ISearchService<CnvProfileIndex> _searchService;

    public CnvProfilesController(ISearchService<CnvProfileIndex> searchService)
    {
        _searchService = searchService;
    }

    [HttpPost("")]
    public async Task<IActionResult> Search(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }
    
    private static SearchResult<CnvProfileResource> From(SearchResult<CnvProfileIndex> searchResult)
    {
        return new SearchResult<CnvProfileResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new CnvProfileResource
            {
                Chromosome =  index.Chromosome,
                ChromosomeArm =  index.ChromosomeArm,
                Gain =  index.Gain,
                Loss =  index.Loss,
                Neutral =  index.Neutral
            }).ToArray()
        };
    }
}