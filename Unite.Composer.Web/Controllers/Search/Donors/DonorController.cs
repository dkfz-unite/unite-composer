using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
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
        private readonly ISearchService<DonorIndex> _donorsSearchService;
        private readonly ISearchService<MutationIndex> _mutationsSearchService;
        private readonly ISearchService<SpecimenIndex, SpecimenSearchContext> _spesimensSearchService;


        public DonorController(
            ISearchService<DonorIndex> donorsSearchService,
            ISearchService<MutationIndex> mutationsSearchService,
            ISearchService<SpecimenIndex, SpecimenSearchContext> specimensSearchService)
        {
            _donorsSearchService = donorsSearchService;
            _mutationsSearchService = mutationsSearchService;
            _spesimensSearchService = specimensSearchService;
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
        public SearchResult<MutationResource> GetMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            searchCriteria.DonorFilters = new DonorCriteria { Id = new[] { id } };

            var searchResult = _mutationsSearchService.Search(searchCriteria);

            return From(searchResult);
        }

        [HttpPost("{id}/specimens")]
        [CookieAuthorize]
        public SearchResult<SpecimenResource> GetSpecimens(int id, [FromBody] SearchCriteria searchCriteria)
        {
            searchCriteria.DonorFilters = new DonorCriteria { Id = new[] { id } };

            var searchContext = new SpecimenSearchContext();

            var searchResult = _spesimensSearchService.Search(searchCriteria, searchContext);

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

        private static SearchResult<MutationResource> From(SearchResult<MutationIndex> searchResult)
        {
            return new SearchResult<MutationResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new MutationResource(index)).ToArray()
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
