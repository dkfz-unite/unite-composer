using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Images;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Web.Controllers.Search.Images
{
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

        [HttpPost("{id}/mutations")]
        public SearchResult<ImageMutationResource> GetMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.SearchMutations(id, searchCriteria);

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

        private static SearchResult<ImageMutationResource> From(int imageId, SearchResult<MutationIndex> searchResult)
        {
            return new SearchResult<ImageMutationResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new ImageMutationResource(imageId, index)).ToArray()
            };
        }
    }
}
