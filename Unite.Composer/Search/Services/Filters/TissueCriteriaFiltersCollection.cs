using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services
{
    public class TissueCriteriaFiltersCollection : SpecimenCriteriaFiltersCollection
    {
        public TissueCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            _filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.TissueIndex>(
                "Specimen.Type",
                specimen => specimen.Tissue)
            );
        }
    }
}
