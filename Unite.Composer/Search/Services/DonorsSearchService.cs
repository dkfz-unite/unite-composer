﻿using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Services.Configuration.Options;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public class DonorsSearchService : IDonorsSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<ImageIndex> _imagesIndexService;
    private readonly IIndexService<SpecimenIndex> _specimensIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;


    public DonorsSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _imagesIndexService = new ImagesIndexService(options);
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
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

    public SearchResult<ImageIndex> SearchImages(int donorId, ImageType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<ImageIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters);

        var result = _imagesIndexService.SearchAsync(query).Result;

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

    public SearchResult<VariantIndex> SearchVariants(int donorId, VariantType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.DonorFilters = new DonorCriteria { Id = new[] { donorId } };

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


    private FiltersCollection<ImageIndex> GetFiltersCollection(ImageType type, SearchCriteria criteria)
    {
        return type switch
        {
            ImageType.MRI => new MriImageIndexFiltersCollection(criteria),
            _ => new ImageIndexFiltersCollection(criteria)
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
