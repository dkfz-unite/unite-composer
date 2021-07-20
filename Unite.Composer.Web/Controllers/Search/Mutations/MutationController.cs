using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Web.Controllers.Search.Mutations
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutationController : Controller
    {
        private readonly ISearchService<DonorIndex> _donorsSearchService;
        private readonly ISearchService<MutationIndex> _mutationsSearchService;


        public MutationController(
            ISearchService<DonorIndex> donorsSearchService,
            ISearchService<MutationIndex> mutationsSearchService)
        {
            _donorsSearchService = donorsSearchService;
            _mutationsSearchService = mutationsSearchService;
        }


        [HttpGet("{id}")]
        [CookieAuthorize]
        public MutationResource Get(long id)
        {
            var key = id.ToString();

            var index = _mutationsSearchService.Get(key);

            return From(index);
        }

        [HttpPost("{id}/donors")]
        [CookieAuthorize]
        public SearchResult<DonorResource> GetDonors(long id, [FromBody] SearchCriteria searchCriteria)
        {
            searchCriteria.MutationFilters = new MutationCriteria { Id = new[] { id } };

            var searchResult = _donorsSearchService.Search(searchCriteria);

            return From(searchResult);
        }


        private static MutationResource From(MutationIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new MutationResource(index);
        }

        private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
        {
            return new SearchResult<DonorResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
            };
        }
    }
}
