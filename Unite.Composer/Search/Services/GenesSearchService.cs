using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services
{
    public class GenesSearchService : IGenesSearchService
    {
        private readonly IIndexService<DonorIndex> _donorsIndexService;
        private readonly IIndexService<GeneIndex> _genesIndexService;
        private readonly IIndexService<MutationIndex> _mutationsIndexService;

        public GenesSearchService(IElasticOptions options)
        {
            _donorsIndexService = new DonorsIndexService(options);
            _genesIndexService = new GenesIndexService(options);
            _mutationsIndexService = new MutationsIndexService(options);
        }

        public GeneIndex Get(string key)
        {
            var query = new GetQuery<GeneIndex>(key)
                .AddExclusion(gene => gene.Mutations);

            var result = _genesIndexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<GeneIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new GeneIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<GeneIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(gene => gene.NumberOfDonors)
                .AddExclusion(gene => gene.Mutations);

            var result = _genesIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.GeneFilters = new GeneCriteria { Id = new[] { geneId } };

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

        public SearchResult<MutationIndex> SearchMutations(int geneId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.GeneFilters = new GeneCriteria { Id = new[] { geneId } };

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
    }
}
