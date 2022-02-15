using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services
{
    public class MutationsSearchService : IMutationsSearchService
    {
        private readonly IIndexService<DonorIndex> _donorsIndexService;
        private readonly IIndexService<MutationIndex> _mutationsIndexService;


        public MutationsSearchService(IElasticOptions options)
        {
            _donorsIndexService = new DonorsIndexService(options);
            _mutationsIndexService = new MutationsIndexService(options);
        }


        public MutationIndex Get(string key)
        {
            var query = new GetQuery<MutationIndex>(key)
                .AddExclusion(mutation => mutation.Donors);

            var result = _mutationsIndexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<MutationIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new MutationIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<MutationIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(mutation => mutation.NumberOfDonors)
                .AddExclusion(mutation => mutation.Donors);

            var result = _mutationsIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<DonorIndex> SearchDonors(long mutationId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.MutationFilters = new MutationCriteria { Id = new[] { mutationId } };

            var criteriaFilters = new DonorIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations)
                .AddExclusion(donor => donor.Specimens);

            var result = _donorsIndexService.SearchAsync(query).Result;

            return result;
        }
    }
}
