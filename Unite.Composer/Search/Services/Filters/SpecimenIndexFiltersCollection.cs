using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class SpecimenIndexFiltersCollection : FiltersCollection<SpecimenIndex>
{
    public SpecimenIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<SpecimenIndex>(criteria.DonorFilters, specimen => specimen.Donor);
        var mriImageFilters = new MriImageFilters<SpecimenIndex>(criteria.MriImageFilters, specimen => specimen.Images.First());
        var geneFilters = new GeneFilters<SpecimenIndex>(criteria.GeneFilters, specimen => specimen.Variants.First().AffectedFeatures.First().Gene);
        var mutationFilters = new MutationFilters<SpecimenIndex>(criteria.MutationFilters, specimen => specimen.Variants.First());
        var copyNumberVariantFilters = new CopyNumberVariantFilters<SpecimenIndex>(criteria.CopyNumberVariantFilters, specimen => specimen.Variants.First());
        var structuralVariantFilters = new StructuralVariantFilters<SpecimenIndex>(criteria.StructuralVariantFilters, specimen => specimen.Variants.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(geneFilters.All());
        _filters.AddRange(mutationFilters.All());
        _filters.AddRange(copyNumberVariantFilters.All());
        _filters.AddRange(structuralVariantFilters.All());
    }
}
