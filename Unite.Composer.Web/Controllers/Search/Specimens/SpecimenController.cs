using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Mutations;
using Unite.Composer.Web.Resources.Specimens;

using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Composer.Web.Controllers.Search.Specimens
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecimenController : Controller
    {
        private readonly ISearchService<SpecimenIndex, SpecimenSearchContext> _specimensSearchService;
        private readonly ISearchService<MutationIndex> _mutationsSearchService;


        public SpecimenController(
            ISearchService<SpecimenIndex, SpecimenSearchContext> specimensSearchService,
            ISearchService<MutationIndex> mutationsSearchService)
        {
            _specimensSearchService = specimensSearchService;
            _mutationsSearchService = mutationsSearchService;
        }


        [HttpGet("{id}")]
        [CookieAuthorize]
        public SpecimenResource Get(int id)
        {
            var key = id.ToString();

            var index = _specimensSearchService.Get(key);

            return From(index);
        }

        [HttpPost("{id}/mutations")]
        [CookieAuthorize]
        public SearchResult<MutationResource> GetMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            searchCriteria.SpecimenFilters = new SpecimenCriteria() { Id = new[] { id } };

            var searchResult = _mutationsSearchService.Search(searchCriteria);

            return From(searchResult);
        }


        private static SpecimenResource From(SpecimenIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new SpecimenResource(index);
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
