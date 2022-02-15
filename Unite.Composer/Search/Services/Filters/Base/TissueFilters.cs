using System;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base
{
    public class TissueFilters<TIndex> : FiltersCollection<TIndex>
        where TIndex : class
    {
        public TissueFilters(TissueCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path)
        {
            if (criteria == null)
            {
                return;
            }

            _filters.Add(new NotNullFilter<TIndex, TissueIndex>(
                SpecimenFilterNames.Type,
                path.Join(specimen => specimen.Tissue))
            );

            _filters.Add(new EqualityFilter<TIndex, int>(
                SpecimenFilterNames.Id,
                path.Join(specimen => specimen.Id),
                criteria.Id)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                TissueFilterNames.ReferenceId,
                path.Join(specimen => specimen.Tissue.ReferenceId),
                criteria.ReferenceId)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.Type,
                path.Join(specimen => specimen.Tissue.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.TumorType,
                path.Join(specimen => specimen.Tissue.TumorType.Suffix(_keywordSuffix)),
                criteria.TumorType)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                TissueFilterNames.Source,
                path.Join(specimen => specimen.Tissue.Source),
                criteria.Source)
            );


            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.MgmtStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.IdhStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.IdhMutation,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.GeneExpressionSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                TissueFilterNames.MethylationSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                TissueFilterNames.GcimpMethylation,
                path.Join(specimen => specimen.CellLine.MolecularData.GcimpMethylation),
                criteria.GcimpMethylation)
            );
        }
    }
}
