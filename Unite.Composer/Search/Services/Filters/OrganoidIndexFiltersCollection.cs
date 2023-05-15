using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class OrganoidIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public OrganoidIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new OrganoidFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen);
        var molecularDataFilters = new MolecularDataFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Organoid);
        var drugScreeningFilters = new DrugScreeningFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Organoid);

        _filters.AddRange(filters.All());
        _filters.AddRange(molecularDataFilters.All());
        _filters.AddRange(drugScreeningFilters.All());

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.OrganoidIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Organoid)
        );
    }
}
