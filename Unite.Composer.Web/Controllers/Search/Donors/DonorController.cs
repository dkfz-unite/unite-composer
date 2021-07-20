using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Composer.Web.Resources.Specimens;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
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


        private static DonorResource From(DonorIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new DonorResource(index);
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
    }
}
