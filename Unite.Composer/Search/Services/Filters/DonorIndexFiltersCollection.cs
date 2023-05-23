using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Search.Services.Filters;

public class DonorIndexFiltersCollection : FiltersCollection<DonorIndex>
{
    public DonorIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<DonorIndex>(criteria.Donor, donor => donor);
        var mriImageFilters = new MriImageFilters<DonorIndex>(criteria.Mri, donor => donor.Images.First());
        var tissueFilters = new TissueFilters<DonorIndex>(criteria.Tissue, donor => donor.Specimens.First());
        var cellLineFilters = new CellLineFilters<DonorIndex>(criteria.Cell, donor => donor.Specimens.First());
        var organoidFilters = new OrganoidFilters<DonorIndex>(criteria.Organoid, donor => donor.Specimens.First());
        var xenograftFilters = new XenograftFilters<DonorIndex>(criteria.Xenograft, donor => donor.Specimens.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());

        if (criteria.Specimen != null)
        {
            _filters.Add(new EqualityFilter<DonorIndex, int>(
                SpecimenFilterNames.Id,
                donor => donor.Specimens.First().Id,
                criteria.Specimen.Id)
            );
        }
    }
}
