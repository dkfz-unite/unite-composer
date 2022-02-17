using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class CellLineIndexFiltersCollection : SpecimenIndexFiltersCollection
    {
        public CellLineIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            var filters = new CellLineFilters<SpecimenIndex>(criteria.CellLineFilters, specimen => specimen);

            _filters.AddRange(filters.All());

            Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.CellLineIndex>(
                SpecimenFilterNames.Type,
                specimen => specimen.CellLine
            ));
        }
    }
}
