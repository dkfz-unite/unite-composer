using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Search.Services.Filters;

public class ImageIndexFiltersCollection : FiltersCollection<ImageIndex>
{
    public ImageIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<ImageIndex>(criteria.DonorFilters, image => image.Donor);
        var tissueFilters = new TissueFilters<ImageIndex>(criteria.TissueFilters, image => image.Specimens.First());
        var cellLineFilters = new CellLineFilters<ImageIndex>(criteria.CellLineFilters, image => image.Specimens.First());
        var organoidFilters = new OrganoidFilters<ImageIndex>(criteria.OrganoidFilters, image => image.Specimens.First());
        var xenograftFilters = new XenograftFilters<ImageIndex>(criteria.XenograftFilters, image => image.Specimens.First());
        var geneFilters = new GeneFilters<ImageIndex>(criteria.GeneFilters, image => image.Specimens.First().Variants.First().AffectedFeatures.First().Gene);
        var mutationFilters = new MutationFilters<ImageIndex>(criteria.MutationFilters, image => image.Specimens.First().Variants.First());
        var copyNumberVariantFilters = new CopyNumberVariantFilters<ImageIndex>(criteria.CopyNumberVariantFilters, image => image.Specimens.First().Variants.First());
        var structuralVariantFilters = new StructuralVariantFilters<ImageIndex>(criteria.StructuralVariantFilters, image => image.Specimens.First().Variants.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());
        _filters.AddRange(geneFilters.All());
        _filters.AddRange(mutationFilters.All());
        _filters.AddRange(copyNumberVariantFilters.All());
        _filters.AddRange(structuralVariantFilters.All());
    }
}
