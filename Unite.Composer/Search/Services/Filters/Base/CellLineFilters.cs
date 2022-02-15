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
    public class CellLineFilters<TIndex> : FiltersCollection<TIndex>
        where TIndex : class
    {
        public CellLineFilters(CellLineCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path)
        {
            if (criteria == null)
            {
                return;
            }

            _filters.Add(new NotNullFilter<TIndex, CellLineIndex>(
                SpecimenFilterNames.Type,
                path.Join(specimen => specimen.CellLine)
            ));

            _filters.Add(new EqualityFilter<TIndex, int>(
                SpecimenFilterNames.Id,
                path.Join(specimen => specimen.Id),
                criteria.Id)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                CellLineFilterNames.ReferenceId,
                path.Join(specimen => specimen.CellLine.ReferenceId),
                criteria.ReferenceId)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.Species,
                path.Join(specimen => specimen.CellLine.Species.Suffix(_keywordSuffix)),
                criteria.Species)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.Type,
                path.Join(specimen => specimen.CellLine.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.CultureType,
                path.Join(specimen => specimen.CellLine.CultureType.Suffix(_keywordSuffix)),
                criteria.CultureType)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                CellLineFilterNames.Name,
                path.Join(specimen => specimen.CellLine.Name),
                criteria.Name)
            );


            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.MgmtStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.IdhStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.IdhMutation,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.GeneExpressionSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.MethylationSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                CellLineFilterNames.GcimpMethylation,
                path.Join(specimen => specimen.CellLine.MolecularData.GcimpMethylation),
                criteria.GcimpMethylation)
            );
        }
    }
}
