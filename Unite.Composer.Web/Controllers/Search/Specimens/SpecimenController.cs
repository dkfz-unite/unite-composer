using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Search.Specimens;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

using DrugScreeningResource = Unite.Composer.Web.Resources.Search.Basic.Specimens.DrugScreeningResource;
using GeneResource = Unite.Composer.Web.Resources.Search.Genes.GeneResource;
using VariantResource = Unite.Composer.Web.Resources.Search.Variants.VariantResource;

namespace Unite.Composer.Web.Controllers.Search.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimenController : Controller
{
    private readonly ISpecimensSearchService _specimensSearchService;
    private readonly DrugScreeningService _drugScreeningService;


    public SpecimenController(
        ISpecimensSearchService specimensSearchService,
        DrugScreeningService drugScreeningService)
    {
        _specimensSearchService = specimensSearchService;
        _drugScreeningService = drugScreeningService;
    }


    [HttpGet("{id}")]
    public SpecimenResource Get(int id)
    {
        var key = id.ToString();

        var index = _specimensSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/genes")]
    public SearchResult<GeneResource> GetGenes(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchGenes(id, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/variants/{type}")]
    public SearchResult<VariantResource> GetVariants(int id, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _specimensSearchService.SearchVariants(id, type, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/drugs")]
    public DrugScreeningResource[] GetDrugsScreeningData(int id)
    {
        var result = _drugScreeningService
            .GetAll(id)
            .Select(model => new DrugScreeningResource(model))
            .ToArray();

        return result;
    }


    private static SpecimenResource From(SpecimenIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new SpecimenResource(index);
    }

    private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<GeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
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
