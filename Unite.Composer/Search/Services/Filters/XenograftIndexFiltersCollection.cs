using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class XenograftIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public XenograftIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new XenograftFilters<SpecimenIndex>(criteria.XenograftFilters, specimen => specimen);

        _filters.AddRange(filters.All());

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.XenograftIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Xenograft)
        );
    }
}
