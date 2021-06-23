using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class CellLineCriteriaFiltersCollection : SpecimenCriteriaFiltersCollection
    {
        public CellLineCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            _filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.CellLineIndex>(
                "Specimen.Type",
                specimen => specimen.CellLine)
            );
        }
    }
}
