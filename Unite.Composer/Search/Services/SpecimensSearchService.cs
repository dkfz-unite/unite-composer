using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Indices.Services.Configuration.Options;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services
{
    public class SpecimensSearchService : ISpecimensSearchService
    {
        private readonly IIndexService<SpecimenIndex> _specimensIndexService;
        private readonly IIndexService<GeneIndex> _genesIndexService;
        private readonly IIndexService<MutationIndex> _mutationsIndexService;


        public SpecimensSearchService(IElasticOptions options)
        {
            _specimensIndexService = new SpecimensIndexService(options);
            _genesIndexService = new GenesIndexService(options);
            _mutationsIndexService = new MutationsIndexService(options);
        }


        public SpecimenIndex Get(string key, SpecimenSearchContext searchContext = null)
        {
            var query = new GetQuery<SpecimenIndex>(key)
                .AddExclusion(specimen => specimen.Mutations);

            var result = _specimensIndexService.GetAsync(query).Result;

            return result;
        }

        public SearchResult<SpecimenIndex> Search(SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var context = searchContext ?? new SpecimenSearchContext();

            var filters = GetFiltersCollection(criteria, context)
                .All();

            var query = new SearchQuery<SpecimenIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(filters)
                .AddOrdering(specimen => specimen.NumberOfMutations)
                .AddExclusion(specimen => specimen.Mutations);

            var result = _specimensIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var context = searchContext ?? new SpecimenSearchContext();

            criteria.SpecimenFilters = new SpecimenCriteria { Id = new[] { specimenId } };

            var criteriaFilters = new GeneCriteriaFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<GeneIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(criteriaFilters)
                .AddOrdering(gene => gene.NumberOfDonors);

            var result = _genesIndexService.SearchAsync(query).Result;

            return result;
        }

        public SearchResult<MutationIndex> SearchMutations(int specimenId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var context = searchContext ?? new SpecimenSearchContext();

            criteria.SpecimenFilters = new SpecimenCriteria { Id = new[] { specimenId } };

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


        private CriteriaFiltersCollection<SpecimenIndex> GetFiltersCollection(SearchCriteria criteria, SpecimenSearchContext context)
        {
            if (context.SpecimenType == Context.Enums.SpecimenType.Tissue)
            {
                return new TissueCriteriaFiltersCollection(criteria);
            }
            else if (context.SpecimenType == Context.Enums.SpecimenType.CellLine)
            {
                return new CellLineCriteriaFiltersCollection(criteria);
            }
            else if (context.SpecimenType == Context.Enums.SpecimenType.Organoid)
            {
                return new OrganoidCriteriaFiltersCollection(criteria);
            }
            else if (context.SpecimenType == Context.Enums.SpecimenType.Xenograft)
            {
                return new XenograftCriteriaFiltersCollection(criteria);
            }
            else
            {
                return new SpecimenCriteriaFiltersCollection(criteria); 
            }
        }
    }
}
