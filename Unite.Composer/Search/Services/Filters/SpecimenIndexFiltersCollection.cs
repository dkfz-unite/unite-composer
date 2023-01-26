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
        var mutationFilters = new MutationFilters<SpecimenIndex>(criteria.MutationFilters, specimen => specimen.Variants.First());
        var mutationGeneFilters = new GeneFilters<SpecimenIndex>(criteria.GeneFilters, specimen => specimen.Variants.First().Mutation.AffectedFeatures.First().Gene);
        var copyNumberVariantFilters = new CopyNumberVariantFilters<SpecimenIndex>(criteria.CopyNumberVariantFilters, specimen => specimen.Variants.First());
        var copyNumberVariantGeneFilters = new GeneFilters<SpecimenIndex>(criteria.GeneFilters, specimen => specimen.Variants.First().CopyNumberVariant.AffectedFeatures.First().Gene);
        var structuralVariantFilters = new StructuralVariantFilters<SpecimenIndex>(criteria.StructuralVariantFilters, specimen => specimen.Variants.First());
        var structuralVariantGeneFilters = new GeneFilters<SpecimenIndex>(criteria.GeneFilters, specimen => specimen.Variants.First().StructuralVariant.AffectedFeatures.First().Gene);

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(mutationFilters.All());
        //_filters.AddRange(mutationGeneFilters.All());
        _filters.AddRange(copyNumberVariantFilters.All());
        //_filters.AddRange(copyNumberVariantGeneFilters.All());
        _filters.AddRange(structuralVariantFilters.All());
        //_filters.AddRange(structuralVariantGeneFilters.All());

        AddWithAnd(
            (mutationGeneFilters.All(), criteria.MutationFilters?.IsNotEmpty() == true),
            (copyNumberVariantGeneFilters.All(), criteria.CopyNumberVariantFilters?.IsNotEmpty() == true),
            (structuralVariantGeneFilters.All(), criteria.StructuralVariantFilters?.IsNotEmpty() == true)
        );
    }
}
