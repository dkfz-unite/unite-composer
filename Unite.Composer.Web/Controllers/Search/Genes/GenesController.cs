﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Genes;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Controllers.Search.Genes
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenesController : Controller
    {
        private readonly IGenesSearchService _searchService;


        public GenesController(IGenesSearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpPost("")]
        [CookieAuthorize]
        public SearchResult<GeneResource> Search([FromBody] SearchCriteria searchCriteria)
        {
            var searchResult = _searchService.Search(searchCriteria);

            return From(searchResult);
        }


        private static SearchResult<GeneResource> From(SearchResult<GeneIndex> searchResult)
        {
            return new SearchResult<GeneResource>()
            {
                Total = searchResult.Total,
                Rows = searchResult.Rows.Select(index => new GeneResource(index)).ToArray()
            };
        }
    }
}
