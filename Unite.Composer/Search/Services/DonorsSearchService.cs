using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Indices.Services.Configuration.Options;

using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Composer.Search.Services
{
    public class DonorsSearchService : IDonorsSearchService
    {
        private readonly IIndexService<DonorIndex> _donorsIndexService;
        private readonly IIndexService<GeneIndex> _genesIndexService;
        private readonly IIndexService<MutationIndex> _mutationsIndexService;
        private readonly IIndexService<SpecimenIndex> _specimensIndexService;
        private readonly IIndexService<ImageIndex> _imagesIndexService;


        public DonorsSearchService(IElasticOptions options)
        {
            _donorsIndexService = new DonorsIndexService(options);
            _genesIndexService = new GenesIndexService(options);
            _mutationsIndexService = new MutationsIndexService(options);
            _specimensIndexService = new SpecimensIndexService(options);
            _imagesIndexService = new ImagesIndexService(options);
        }


        public DonorIndex Get(string key)
        {
            var query = new GetQuery<DonorIndex>(key)
                .AddExclusion(donor => donor.Specimens);

            var result = _donorsIndexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<DonorIndex> Search(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var criteriaFilters = new DonorIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(donor => donor.NumberOfMutations)
                .AddExclusion(donor => donor.Specimens)
                .AddExclusion(donor => donor.Images);

            var result = _donorsIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<GeneIndex> SearchGenes(int donorId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

            var criteriaFilters = new GeneIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<GeneIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(gene => gene.NumberOfDonors);

            var result = _genesIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<MutationIndex> SearchMutations(int donorId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

            var criteriaFilters = new MutationIndexFiltersCollection(criteria)
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

            var criteriaFilters = new SpecimenIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<SpecimenIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(specimen => specimen.NumberOfMutations);

            var result = _specimensIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<ImageIndex> SearchImages(int donorId, SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

            var criteriaFilters = new ImageIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<ImageIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters);

            var result = _imagesIndexService.SearchAsync(query).Result;

            return result;
        }
    }
}
