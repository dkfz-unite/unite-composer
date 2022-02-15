using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services
{
    public class TissueIndexFiltersCollection : SpecimenIndexFiltersCollection
    {
        public TissueIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            var filters = new TissueFilters<SpecimenIndex>(criteria.TissueFilters, specimen => specimen);

            _filters.AddRange(filters.All());
        }
    }
}
