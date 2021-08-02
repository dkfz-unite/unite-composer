using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Composer.Search.Services
{
    public class DonorsSearchService : IDonorsSearchService
    {
        private readonly IIndexService<DonorIndex> _donorsIndexService;
        private readonly IIndexService<MutationIndex> _mutationsIndexService;
        private readonly IIndexService<SpecimenIndex> _specimensIndexService;


        public DonorsSearchService(IElasticOptions options)
        {
            _donorsIndexService = new DonorsIndexService(options);
            _mutationsIndexService = new MutationsIndexService(options);
            _specimensIndexService = new SpecimensIndexService(options);
        }


        public DonorIndex Get(string key)
        {
            var query = new GetQuery<DonorIndex>(key)
                .AddExclusion(donor => donor.Mutations);

            var result = _donorsIndexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<DonorIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new DonorCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations)
                .AddExclusion(donor => donor.Mutations);

            var result = _donorsIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<MutationIndex> SearchMutations(int donorId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

            var criteriaFilters = new MutationCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<MutationIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(mutation => mutation.NumberOfDonors);

            var result = _mutationsIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

            var criteriaFilters = new SpecimenCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<SpecimenIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(specimen => specimen.NumberOfMutations);

            var result = _specimensIndexService.SearchAsync(query).Result;

            return result;
        }
    }
}
