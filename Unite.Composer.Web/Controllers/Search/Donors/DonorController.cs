using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Composer.Web.Resources.Images;
using Unite.Composer.Web.Resources.Specimens;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Composer.Web.Controllers.Search.Donors
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : Controller
    {
        private readonly IDonorsSearchService _donorsSearchService;


        public DonorController(
            IDonorsSearchService donorsSearchService)
        {
            _donorsSearchService = donorsSearchService;
        }


        [HttpGet("{id}")]
        [CookieAuthorize]
        public DonorResource Get(int id)
        {
            var key = id.ToString();

            var index = _donorsSearchService.Get(key);

            return From(index);
        }

        [HttpPost("{id}/genes")]
        [CookieAuthorize]
        public SearchResult<DonorGeneResource> SearchGenes(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _donorsSearchService.SearchGenes(id, searchCriteria);

            return From(id, searchResult);
        }

        [HttpPost("{id}/mutations")]
        [CookieAuthorize]
        public SearchResult<DonorMutationResource> SearchMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _donorsSearchService.SearchMutations(id, searchCriteria);

            return From(id, searchResult);
        }

        [HttpPost("{id}/specimens")]
        [CookieAuthorize]
        public SearchResult<SpecimenResource> SearchSpecimens(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _donorsSearchService.SearchSpecimens(id, searchCriteria);

            return From(searchResult);
        }

        [HttpPost("{id}/images/{type}")]
        [CookieAuthorize]
        public SearchResult<ImageResource> SearchImages(int id, ImageType type, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _donorsSearchService.SearchImages(id, type, searchCriteria);

            return From(searchResult);
        }


        private static DonorResource From(DonorIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new DonorResource(index);
        }

        private static SearchResult<DonorGeneResource> From(int donorId, SearchResult<GeneIndex> searchResult)
        {
            return new SearchResult<DonorGeneResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorGeneResource(donorId, index)).ToArray()
            };
        }

        private static SearchResult<DonorMutationResource> From(int donorId, SearchResult<MutationIndex> searchResult)
        {
            return new SearchResult<DonorMutationResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorMutationResource(donorId, index)).ToArray()
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
