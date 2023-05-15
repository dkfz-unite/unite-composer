using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class SpecimenIndexFiltersCollection : FiltersCollection<SpecimenIndex>
{
    public SpecimenIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<SpecimenIndex>(criteria.Donor, specimen => specimen.Donor);
        var mriImageFilters = new MriImageFilters<SpecimenIndex>(criteria.Mri, specimen => specimen.Images.First());
        
        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
    }
}
