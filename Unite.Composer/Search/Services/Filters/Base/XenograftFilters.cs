using System;
using System.Linq;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base
{
    public class XenograftFilters<TIndex> : FiltersCollection<TIndex>
        where TIndex : class
    {
        public XenograftFilters(XenograftCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path)
        {
            if (criteria == null)
            {
                return;
            }

            _filters.Add(new NotNullFilter<TIndex, XenograftIndex>(
                SpecimenFilterNames.Type,
                path.Join(specimen => specimen.Xenograft))
            );

            _filters.Add(new EqualityFilter<TIndex, int>(
                SpecimenFilterNames.Id,
                path.Join(specimen => specimen.Id),
                criteria.Id)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                XenograftFilterNames.ReferenceId,
                path.Join(specimen => specimen.Xenograft.ReferenceId),
                criteria.ReferenceId)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                XenograftFilterNames.MouseStrain,
                path.Join(specimen => specimen.Xenograft.MouseStrain),
                criteria.MouseStrain)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                XenograftFilterNames.Tumorigenicity,
                path.Join(specimen => specimen.Xenograft.Tumorigenicity),
                criteria.Tumorigenicity)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                XenograftFilterNames.TumorGrowthForm,
                path.Join(specimen => specimen.Xenograft.TumorGrowthForm.Suffix(_keywordSuffix)),
                criteria.TumorGrowthForm)
            );

            _filters.Add(new MultiPropertyRangeFilter<TIndex, int?>(
                XenograftFilterNames.SurvivalDays,
                path.Join(specimen => specimen.Xenograft.SurvivalDaysFrom),
                path.Join(specimen => specimen.Xenograft.SurvivalDaysTo),
                criteria.SurvivalDays?.From,
                criteria.SurvivalDays?.To)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                XenograftFilterNames.Intervention,
                path.Join(specimen => specimen.Xenograft.Interventions.First().Type),
                criteria.Intervention)
            );


            _filters.Add(new EqualityFilter<TIndex, object>(
                XenograftFilterNames.MgmtStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                XenograftFilterNames.IdhStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                XenograftFilterNames.IdhMutation,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                XenograftFilterNames.GeneExpressionSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                XenograftFilterNames.MethylationSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                XenograftFilterNames.GcimpMethylation,
                path.Join(specimen => specimen.CellLine.MolecularData.GcimpMethylation),
                criteria.GcimpMethylation)
            );
        }
    }
}
