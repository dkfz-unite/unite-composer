using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Services.Configuration.Options;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class SpecimensSearchService : AggregatingSearchService, ISpecimensSearchService
{
    private readonly IIndexService<SpecimenIndex> _specimensIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;


    public SpecimensSearchService(IElasticOptions options)
    {
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public SpecimenIndex Get(string key, SpecimenSearchContext searchContext = null)
    {
        var query = new GetQuery<SpecimenIndex>(key);

        var result = _specimensIndexService.GetAsync(query).Result;

        return result;
    }

    public SearchResult<SpecimenIndex> Search(SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new SpecimenSearchContext();

        var ids =  AggregateFromVariants(index => index.Samples.First().Id, criteria)
                ?? AggregateFromGenes(index => index.Samples.First().Id, criteria)
                ?? null;

        if (ids?.Length == 0)
        {
            return new SearchResult<SpecimenIndex>();
        }
        else
        {
            if (ids != null)
            {
                criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Id = ids };
            }

            var filters = GetFiltersCollection(criteria, context)
                .All();

            var query = new SearchQuery<SpecimenIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(filters)
                .AddOrdering(specimen => specimen.NumberOfGenes)
                .AddExclusion(specimen => specimen.Cell.DrugScreenings)
                .AddExclusion(specimen => specimen.Organoid.DrugScreenings)
                .AddExclusion(specimen => specimen.Xenograft.DrugScreenings)
                .AddExclusion(specimen => specimen.Images);

            var result = _specimensIndexService.SearchAsync(query).Result;

            return result;
        }
    }

    public SearchResult<GeneIndex> SearchGenes(int sampleId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new SpecimenSearchContext();

        criteria.Sample = new SampleCriteria { Id = new[] { sampleId } };

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

    public SearchResult<VariantIndex> SearchVariants(int sampleId, VariantType type, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new SpecimenSearchContext();

        criteria.Sample = new SampleCriteria { Id = new[] { sampleId } };

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(mutation => mutation.NumberOfDonors);

        var result = _variantsIndexService.SearchAsync(query).Result;

        return result;
    }


    private FiltersCollection<SpecimenIndex> GetFiltersCollection(SearchCriteria criteria, SpecimenSearchContext context)
    {
        return context.SpecimenType switch
        {
            Context.Enums.SpecimenType.Tissue => new TissueIndexFiltersCollection(criteria),
            Context.Enums.SpecimenType.CellLine => new CellLineIndexFiltersCollection(criteria),
            Context.Enums.SpecimenType.Organoid => new OrganoidIndexFiltersCollection(criteria),
            Context.Enums.SpecimenType.Xenograft => new XenograftIndexFiltersCollection(criteria),
            _ => new SpecimenIndexFiltersCollection(criteria)
        };
    }

    private FiltersCollection<VariantIndex> GetFiltersCollection(VariantType type, SearchCriteria criteria)
    {
        return type switch
        {
            VariantType.SSM => new MutationIndexFiltersCollection(criteria),
            VariantType.CNV => new CopyNumberVariantFiltersCollection(criteria),
            VariantType.SV => new StructuralVariantFiltersCollection(criteria),
            _ => new VariantFiltersCollection(criteria)
        };
    }
}
