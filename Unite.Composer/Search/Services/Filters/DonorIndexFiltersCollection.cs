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
        var donorFilters = new DonorFilters<DonorIndex>(criteria.DonorFilters, donor => donor);
        var mriImageFilters = new MriImageFilters<DonorIndex>(criteria.MriImageFilters, donor => donor.Images.First());
        var tissueFilters = new TissueFilters<DonorIndex>(criteria.TissueFilters, donor => donor.Specimens.First());
        var cellLineFilters = new CellLineFilters<DonorIndex>(criteria.CellLineFilters, donor => donor.Specimens.First());
        var organoidFilters = new OrganoidFilters<DonorIndex>(criteria.OrganoidFilters, donor => donor.Specimens.First());
        var xenograftFilters = new XenograftFilters<DonorIndex>(criteria.XenograftFilters, donor => donor.Specimens.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());

        if (criteria.SpecimenFilters != null)
        {
            _filters.Add(new EqualityFilter<DonorIndex, int>(
                SpecimenFilterNames.Id,
                donor => donor.Specimens.First().Id,
                criteria.SpecimenFilters.Id)
            );
        }
    }
}
