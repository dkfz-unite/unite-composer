using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class CellLineIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public CellLineIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new CellLineFilters<SpecimenIndex>(criteria.Cell, specimen => specimen);
        var molecularDataFilters = new MolecularDataFilters<SpecimenIndex>(criteria.Cell, specimen => specimen.Cell.MolecularData);
        var drugScreeningFilters = new DrugScreeningFilters<SpecimenIndex>(criteria.Cell, specimen => specimen.Cell.DrugScreenings);
        var availableDataFilters = new SpecimenDataFilters<SpecimenIndex>(criteria.Cell, specimen => specimen.Data);

        _filters.AddRange(filters.All());
        _filters.AddRange(molecularDataFilters.All());
        _filters.AddRange(drugScreeningFilters.All());
        _filters.AddRange(availableDataFilters.All());

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.CellLineIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Cell
        ));
    }
}
