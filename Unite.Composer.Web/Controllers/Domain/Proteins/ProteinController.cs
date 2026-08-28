using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Web.Extensions;
using Unite.Composer.Web.Resources.Domain.Proteins;
using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Criteria;


namespace Unite.Composer.Web.Controllers.Domain.Proteins;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProteinController : DomainController
{
    private readonly ISearchService<ProteinIndex> _proteinsSearchService;


    public ProteinController(
        ISearchService<ProteinIndex> proteinsSearchService)
    {
        _proteinsSearchService = proteinsSearchService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Protein(int id)
    {
        var key = id.ToString();

        var result = await _proteinsSearchService.Get(new PersonalGetCriteria(key, new UserClaims(User.GetUserId(), User.GetIsRoot())));

        return Ok(From(result));
    }


    private static ProteinResource From(ProteinIndex index)
    {
        if (index == null)
            return null;

        return new ProteinResource(index);
    }
}
