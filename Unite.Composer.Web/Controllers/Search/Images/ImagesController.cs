using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Images;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Controllers.Search.Images
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : Controller
    {
        private readonly IImagesSearchService _searchService;


        public ImagesController(IImagesSearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpPost("{type}")]
        public SearchResult<ImageResource> Search(ImageType type, [FromBody] SearchCriteria searchCriteria)
        {
            var searchContext = new ImageSearchContext(type);

            var searchResult = _searchService.Search(searchCriteria, searchContext);

            return From(searchResult);
        }

        private static SearchResult<ImageResource> From(SearchResult<ImageIndex> searchResult)
        {
            return new SearchResult<ImageResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new ImageResource(index)).ToArray()
            };
        }
    }
}
