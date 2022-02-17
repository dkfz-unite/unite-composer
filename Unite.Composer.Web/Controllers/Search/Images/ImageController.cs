using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Images;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using GeneResource = Unite.Composer.Web.Resources.Genes.GeneResource;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using MutationResource = Unite.Composer.Web.Resources.Mutations.MutationResource;

namespace Unite.Composer.Web.Controllers.Search.Images
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImagesSearchService _searchService;


        public ImageController(IImagesSearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpGet("{id}")]
        [CookieAuthorize]
        public ImageResource Get(int id)
        {
            var key = id.ToString();

            var index = _searchService.Get(key);

            return From(index);
        }

        [HttpPost("{id}/genes")]
        [CookieAuthorize]
        public SearchResult<GeneResource> GetGenes(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.SearchGenes(id, searchCriteria);

            return From(searchResult);
        }

        [HttpPost("{id}/mutations")]
        [CookieAuthorize]
        public SearchResult<MutationResource> GetMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.SearchMutations(id, searchCriteria);

            return From(searchResult);
        }


        private static ImageResource From(ImageIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new ImageResource(index);
        }

        private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
        {
            return new SearchResult<GeneResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
            };
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
}
