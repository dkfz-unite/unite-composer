using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class XenograftCriteriaFiltersCollection : SpecimenCriteriaFiltersCollection
    {
        public XenograftCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            _filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.XenograftIndex>(
                "Specimen.Type",
                specimen => specimen.Xenograft)
            );
        }
    }
}
