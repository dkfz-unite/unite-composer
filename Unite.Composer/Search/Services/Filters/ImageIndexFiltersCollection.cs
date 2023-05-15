using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Search.Services.Filters;

public class ImageIndexFiltersCollection : FiltersCollection<ImageIndex>
{
    public ImageIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<ImageIndex>(criteria.Donor, image => image.Donor);
        var tissueFilters = new TissueFilters<ImageIndex>(criteria.Tissue, image => image.Specimens.First());
        var cellLineFilters = new CellLineFilters<ImageIndex>(criteria.Cell, image => image.Specimens.First());
        var organoidFilters = new OrganoidFilters<ImageIndex>(criteria.Organoid, image => image.Specimens.First());
        var xenograftFilters = new XenograftFilters<ImageIndex>(criteria.Xenograft, image => image.Specimens.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());
    }
}
