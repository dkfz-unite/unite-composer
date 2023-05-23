using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Search.Services.Filters;

public class VariantFiltersCollection : FiltersCollection<VariantIndex>
{
    public VariantFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<VariantIndex>(criteria.Donor, variant => variant.Samples.First().Donor);
        var mriImageFilters = new MriImageFilters<VariantIndex>(criteria.Mri, variant => variant.Samples.First().Images.First());
        var tissueFilters = new TissueFilters<VariantIndex>(criteria.Tissue, variant => variant.Samples.First().Specimen);
        var cellLineFilters = new CellLineFilters<VariantIndex>(criteria.Cell, variant => variant.Samples.First().Specimen);
        var organoidFilters = new OrganoidFilters<VariantIndex>(criteria.Organoid, variant => variant.Samples.First().Specimen);
        var xenograftFilters = new XenograftFilters<VariantIndex>(criteria.Xenograft, variant => variant.Samples.First().Specimen);

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());

        if (criteria.Image != null)
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
              ImageFilterNames.Id,
              variant => variant.Samples.First().Images.First().Id,
              criteria.Image.Id)
            );
        }

        if (criteria.Specimen != null)
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
                SpecimenFilterNames.Id,
                variant => variant.Samples.First().Specimen.Id,
                criteria.Specimen.Id)
            );
        }

        if (criteria.Sample != null)
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
                "Sample.Id",
                variant => variant.Samples.First().Id,
                criteria.Sample.Id
            ));
        }
    }
}
