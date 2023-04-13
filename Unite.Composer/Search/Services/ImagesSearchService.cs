using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Services.Configuration.Options;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class ImagesSearchService : AggregatingSearchService, IImagesSearchService
{
    private readonly IIndexService<ImageIndex> _imagesIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;


    public ImagesSearchService(IElasticOptions options)
    {
        _imagesIndexService = new ImagesIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public ImageIndex Get(string key, ImageSearchContext searchContext = null)
    {
        var query = new GetQuery<ImageIndex>(key)
            .AddExclusion(image => image.Donor)
            .AddExclusion(image => image.Specimens);

        var result = _imagesIndexService.GetAsync(query).Result;

        return result;
    }

    public SearchResult<ImageIndex> Search(SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new ImageSearchContext();

        var filters = GetFiltersCollection(criteria, context)
            .All();

        var ids = AggregateFromVariants(index => index.Samples.First().Images.First().Id, criteria)
               ?? AggregateFromGenes(index => index.Samples.First().Images.First().Id, criteria)
               ?? null;

        if (ids?.Length == 0)
        {
            return new SearchResult<ImageIndex>();
        }
        else
        {
            if (ids != null)
            {
                criteria.ImageFilters = (criteria.ImageFilters ?? new ImageCriteria()) with { Id = ids };
            }

            var query = new SearchQuery<ImageIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(filters)
                .AddOrdering(image => image.Id, true)
                .AddExclusion(image => image.Specimens);

            var result = _imagesIndexService.SearchAsync(query).Result;

            return result;
        }
    }

    public SearchResult<GeneIndex> SearchGenes(int sampleId, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new ImageSearchContext();

        criteria.SampleFilters = new SampleCriteria { Id = new[] { sampleId }};

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

    public SearchResult<VariantIndex> SearchVariants(int sampleId, VariantType type, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new ImageSearchContext();

        criteria.SampleFilters = new SampleCriteria { Id = new[] { sampleId }};

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


    private FiltersCollection<ImageIndex> GetFiltersCollection(SearchCriteria criteria, ImageSearchContext context)
    {
        return context.ImageType switch
        {
            Context.Enums.ImageType.MRI => new MriImageIndexFiltersCollection(criteria),
            Context.Enums.ImageType.CT => throw new NotImplementedException(),
            _ => new ImageIndexFiltersCollection(criteria),
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
