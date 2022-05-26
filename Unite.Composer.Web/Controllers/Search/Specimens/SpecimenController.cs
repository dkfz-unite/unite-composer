using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Resources.Specimens;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using GeneResource = Unite.Composer.Web.Resources.Genes.GeneResource;
using MutationResource = Unite.Composer.Web.Resources.Mutations.MutationResource;
using Microsoft.AspNetCore.Authorization;

namespace Unite.Composer.Web.Controllers.Search.Specimens
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecimenController : Controller
    {
        private readonly ISpecimensSearchService _specimensSearchService;


        public SpecimenController(
            ISpecimensSearchService specimensSearchService)
        {
            _specimensSearchService = specimensSearchService;
        }


        [HttpGet("{id}")]
        public SpecimenResource Get(int id)
        {
            var key = id.ToString();

            var index = _specimensSearchService.Get(key);

            return From(index);
        }

        [HttpPost("{id}/genes")]
        public SearchResult<GeneResource> GetGenes(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _specimensSearchService.SearchGenes(id, searchCriteria);

            return From(searchResult);
        }

        [HttpPost("{id}/mutations")]
        public SearchResult<MutationResource> GetMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _specimensSearchService.SearchMutations(id, searchCriteria);

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
