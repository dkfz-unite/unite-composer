using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
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


            if (criteria.XenograftFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                   XenograftFilterNames.Id,
                   specimen => specimen.Id,
                   criteria.XenograftFilters.Id)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    XenograftFilterNames.ReferenceId,
                    specimen => specimen.Xenograft.ReferenceId,
                    criteria.XenograftFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    XenograftFilterNames.MouseStrain,
                    specimen => specimen.Xenograft.MouseStrain,
                    criteria.XenograftFilters.MouseStrain)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    XenograftFilterNames.Tumorigenicity,
                    specimen => specimen.Xenograft.Tumorigenicity,
                    criteria.XenograftFilters.Tumorigenicity)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.TumorGrowthForm,
                    specimen => specimen.Xenograft.TumorGrowthForm.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.TumorGrowthForm)
                );

                _filters.Add(new MultiPropertyRangeFilter<SpecimenIndex, double?>(
                    XenograftFilterNames.SurvivalDays,
                    specimen => specimen.Xenograft.SurvivalDaysFrom,
                    specimen => specimen.Xenograft.SurvivalDaysTo,
                    criteria.XenograftFilters.SurvivalDays?.From,
                    criteria.XenograftFilters.SurvivalDays?.To)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    XenograftFilterNames.Intervention,
                    specimen => specimen.Xenograft.Interventions.First().Type,
                    criteria.XenograftFilters.Intervention)
                );


                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.MgmtStatus,
                    specimen => specimen.Xenograft.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.IdhStatus,
                    specimen => specimen.Xenograft.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.IdhMutation,
                    specimen => specimen.Xenograft.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.GeneExpressionSubtype,
                    specimen => specimen.Xenograft.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.MethylationSubtype,
                    specimen => specimen.Xenograft.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    XenograftFilterNames.GcimpMethylation,
                    specimen => specimen.Xenograft.MolecularData.GcimpMethylation,
                    criteria.XenograftFilters.GcimpMethylation)
                );
            }
        }
    }
}
