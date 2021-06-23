using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class OrganoidCriteriaFiltersCollection : SpecimenCriteriaFiltersCollection
    {
        public OrganoidCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            _filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.OrganoidIndex>(
                "Specimen.Type",
                specimen => specimen.Organoid)
            );
        }
    }
}
