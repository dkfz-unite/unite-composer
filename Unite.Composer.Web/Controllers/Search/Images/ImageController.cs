using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Search.Images;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Web.Controllers.Search.Images;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageController : Controller
{
    private readonly IImagesSearchService _searchService;


    public ImageController(IImagesSearchService searchService)
    {
        _searchService = searchService;
    }


    [HttpGet("{id}")]
    public ImageResource Get(int id)
    {
        var key = id.ToString();

        var index = _searchService.Get(key);

        return From(index);
    }

    [HttpPost("{id}/genes")]
    public SearchResult<ImageGeneResource> GetGenes(int id, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.SearchGenes(id, searchCriteria);

        return From(id, searchResult);
    }

    [HttpPost("{id}/variants/{type}")]
    public SearchResult<ImageVariantResource> GetMutations(int id, VariantType type, [FromBody] SearchCriteria searchCriteria)
    {
        var searchResult = _searchService.SearchVariants(id, type, searchCriteria);

        return From(id, searchResult);
    }


    private static ImageResource From(ImageIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new ImageResource(index);
    }

    private static SearchResult<ImageGeneResource> From(int imageId, SearchResult<GeneIndex> searchResult)
    {
        return new SearchResult<ImageGeneResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ImageGeneResource(imageId, index)).ToArray()
        };
    }

    private static SearchResult<ImageVariantResource> From(int imageId, SearchResult<VariantIndex> searchResult)
    {
        return new SearchResult<ImageVariantResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new ImageVariantResource(imageId, index)).ToArray()
        };
    }
}
