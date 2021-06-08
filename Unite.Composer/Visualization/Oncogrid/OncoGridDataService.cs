using System.Collections.Generic;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Composer.Visualization.Oncogrid.Models;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Visualization.Oncogrid
{
    public class OncogridDataService
    {
        private readonly IIndexService<DonorIndex> _indexService;


        public OncogridDataService(IElasticOptions options)
        {
            _indexService = new DonorsIndexService(options);
        }


        //TODO: Create oncogrid search criteria similar to search criteria if required
        public OncogridData GetData(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new DonorCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfGenes);

            var result = _indexService.SearchAsync(query).Result;

            return From(result.Rows);
        }


        private OncogridData From(IEnumerable<DonorIndex> indices)
        {
            //TODO: Create data from indices
            return new OncogridData();
        }
    }
}
