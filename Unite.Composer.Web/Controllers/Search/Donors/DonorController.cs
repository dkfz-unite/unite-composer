using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Search.Donors;
using Unite.Composer.Web.Resources.Search.Genes;
using Unite.Composer.Web.Resources.Search.Images;
using Unite.Composer.Web.Resources.Search.Specimens;
using Unite.Composer.Web.Resources.Search.Variants;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Search.Donors;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DonorController : Controller
{
    private readonly IDonorsSearchService _donorsSearchService;


    public DonorController(
        IDonorsSearchService donorsSearchService)
    {
        _donorsSearchService = donorsSearchService;
    }


    [HttpGet("{id}")]
    public DonorResource Get(int id)
    {
        var key = id.ToString();

        var index = _donorsSearchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/images/{type}")]
    public SearchResult<ImageResource> SearchImages(int id, ImageType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchImages(id, type, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/specimens")]
    public SearchResult<SpecimenResource> SearchSpecimens(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchSpecimens(id, searchCriteria);

        return From(searchResult);
    }

    [HttpPost("{id}/genes")]
    public SearchResult<DonorGeneResource> SearchGenes(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchGenes(id, searchCriteria);

        return From(id, searchResult);
    }

    [HttpPost("{id}/variants/{type}")]
    public SearchResult<VariantResource> SearchMutations(int id, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _donorsSearchService.SearchVariants(id, type, searchCriteria);

        return From(id, searchResult);
    }


    private static DonorResource From(DonorIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new DonorResource(index);
    }

    private static SearchResult<ImageResource> From(SearchResult<ImageIndex> searchResult)
    {
        return new SearchResult<ImageResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ImageResource(index)).ToArray()
        };
    }

    private static SearchResult<SpecimenResource> From(SearchResult<SpecimenIndex> searchResult)
    {
        return new SearchResult<SpecimenResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SpecimenResource(index)).ToArray()
        };
    }

    private static SearchResult<DonorGeneResource> From(int donorId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<DonorGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new DonorGeneResource(index, donorId)).ToArray()
        };
    }

    private static SearchResult<VariantResource> From(int donorId, SearchResult<VariantIndex> searchResult)
    {
        return new SearchResult<VariantResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new VariantResource(index)).ToArray()
        };
    }
}
