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
        var donorFilters = new DonorFilters<VariantIndex>(criteria.DonorFilters, variant => variant.Specimens.First().Donor);
        var mriImageFilters = new MriImageFilters<VariantIndex>(criteria.MriImageFilters, variant => variant.Specimens.First().Images.First());
        var tissueFilters = new TissueFilters<VariantIndex>(criteria.TissueFilters, variant => variant.Specimens.First());
        var cellLineFilters = new CellLineFilters<VariantIndex>(criteria.CellLineFilters, variant => variant.Specimens.First());
        var organoidFilters = new OrganoidFilters<VariantIndex>(criteria.OrganoidFilters, variant => variant.Specimens.First());
        var xenograftFilters = new XenograftFilters<VariantIndex>(criteria.XenograftFilters, variant => variant.Specimens.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());

        if (criteria.ImageFilters != null)
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
              ImageFilterNames.Id,
              variant => variant.Specimens.First().Images.First().Id,
              criteria.ImageFilters.Id)
            );
        }

        if (criteria.SpecimenFilters != null)
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
                SpecimenFilterNames.Id,
                variant => variant.Specimens.First().Id,
                criteria.SpecimenFilters.Id)
            );
        }
    }
}
