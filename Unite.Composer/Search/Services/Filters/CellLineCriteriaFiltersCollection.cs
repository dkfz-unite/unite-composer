using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class CellLineCriteriaFiltersCollection : SpecimenCriteriaFiltersCollection
    {
        public CellLineCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            _filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.CellLineIndex>(
                SpecimenFilterNames.Type,
                specimen => specimen.CellLine)
            );

            if (criteria.CellLineFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                    SpecimenFilterNames.Id,
                    specimen => specimen.Id,
                    criteria.CellLineFilters.Id)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    specimen => specimen.CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.Species,
                    specimen => specimen.CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.Type,
                    specimen => specimen.CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.CultureType,
                    specimen => specimen.CellLine.CultureType.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.CultureType)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    CellLineFilterNames.Name,
                    specimen => specimen.CellLine.Name,
                    criteria.CellLineFilters.Name)
                );


                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.MgmtStatus,
                    specimen => specimen.CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.IdhStatus,
                    specimen => specimen.CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.IdhMutation,
                    specimen => specimen.CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.GeneExpressionSubtype,
                    specimen => specimen.CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.MethylationSubtype,
                    specimen => specimen.CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    CellLineFilterNames.GcimpMethylation,
                    specimen => specimen.CellLine.MolecularData.GcimpMethylation,
                    criteria.CellLineFilters.GcimpMethylation)
                );
            }
        }
    }
}
