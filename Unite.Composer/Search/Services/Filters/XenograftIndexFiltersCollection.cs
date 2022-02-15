using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class XenograftIndexFiltersCollection : SpecimenIndexFiltersCollection
    {
        public XenograftIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            var filters = new XenograftFilters<SpecimenIndex>(criteria.XenograftFilters, specimen => specimen);

            _filters.AddRange(filters.All());
        }
    }
}
