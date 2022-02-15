using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class OrganoidIndexFiltersCollection : SpecimenIndexFiltersCollection
    {
        public OrganoidIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            var filters = new OrganoidFilters<SpecimenIndex>(criteria.OrganoidFilters, specimen => specimen);

            _filters.AddRange(filters.All());
        }
    }
}
