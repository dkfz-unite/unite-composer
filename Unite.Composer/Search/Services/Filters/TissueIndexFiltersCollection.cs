using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class TissueIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public TissueIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new TissueFilters<SpecimenIndex>(criteria.Tissue, specimen => specimen);
        var molecularDataFilters = new MolecularDataFilters<SpecimenIndex>(criteria.Tissue, specimen => specimen.Tissue);

        _filters.AddRange(filters.All());
        _filters.AddRange(molecularDataFilters.All());

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.TissueIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Tissue)
        );
    }
}
