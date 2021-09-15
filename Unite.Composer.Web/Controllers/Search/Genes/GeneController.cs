using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Composer.Web.Resources.Mutations;

using GeneResource = Unite.Composer.Web.Resources.Genes.GeneResource;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Web.Controllers.Search.Genes
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneController : Controller
    {
        private readonly IGenesSearchService _genesSearchService;


        public GeneController(IGenesSearchService genesSearchService)
        {
            _genesSearchService = genesSearchService;
        }


        [HttpGet("{id}")]
        [CookieAuthorize]
        public GeneResource Get(long id)
        {
            var key = id.ToString();

            var index = _genesSearchService.Get(key);

            return From(index);
        }

        [HttpPost("{id}/donors")]
        [CookieAuthorize]
        public SearchResult<DonorResource> GetDonors(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _genesSearchService.SearchDonors(id, searchCriteria);

            return From(searchResult);
        }

        [HttpPost("{id}/mutations")]
        [CookieAuthorize]
        public SearchResult<MutationResource> GetMutations(int id, [FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _genesSearchService.SearchMutations(id, searchCriteria);

            return From(searchResult);
        }


        private static GeneResource From(GeneIndex index)
        {
            if (index == null)
            {
                return null;
            }

            return new GeneResource(index);
        }

        private static SearchResult<DonorResource> From(SearchResult<DonorIndex> searchResult)
        {
            return new SearchResult<DonorResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new DonorResource(index)).ToArray()
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
