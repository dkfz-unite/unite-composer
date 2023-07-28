using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Indices.Entities.Genes;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class GenesSearchService : AggregatingSearchService, IGenesSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;

    public GenesSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }

    public GeneIndex Get(string key)
    {
        var query = new GetQuery<GeneIndex>(key)
            .AddExclusion(gene => gene.Samples);

        var result = _genesIndexService.GetAsync(query).Result;

        return result;
    }

    public IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var availableData = new Dictionary<int, DataIndex>();

        criteria = criteria with { From = 0, Size = 0 };
        var lookupResult = Search(criteria);

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };
            var searchResult = Search(criteria);

            foreach (var index in searchResult.Rows)
            {
                availableData.Add(index.Id, index.Data);
            }
        }

        return availableData;
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
            .AddExclusion(gene => gene.Samples);

        var result = _genesIndexService.SearchAsync(query).Result;

        return result;
    }

    public SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Gene = new GeneCriteria() { Id = new[] { geneId } };

        // Should never be null or empty
        var ids = AggregateFromGenes(index => index.Samples.First().Donor.Id, criteria);

        criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };

        var filters = new DonorIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFilters(filters)
            .AddOrdering(donor => donor.NumberOfGenes);

        var result = _donorsIndexService.SearchAsync(query).Result;

        return result;
    }

    public SearchResult<VariantIndex> SearchVariants(int geneId, VariantType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Gene = new GeneCriteria { Id = new[] { geneId } };

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(mutation => mutation.NumberOfDonors)
            .AddExclusion(mutation => mutation.Samples);

        var result = _variantsIndexService.SearchAsync(query).Result;

        return result;
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
