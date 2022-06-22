using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class CellLineFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public CellLineFilters(CellLineCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, int>(
            SpecimenFilterNames.Id,
            path.Join(specimen => specimen.Id),
            criteria.Id)
        );

        Add(new SimilarityFilter<TIndex, string>(
            CellLineFilterNames.ReferenceId,
            path.Join(specimen => specimen.CellLine.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.Species,
            path.Join(specimen => specimen.CellLine.Species.Suffix(_keywordSuffix)),
            criteria.Species)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.Type,
            path.Join(specimen => specimen.CellLine.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.CultureType,
            path.Join(specimen => specimen.CellLine.CultureType.Suffix(_keywordSuffix)),
            criteria.CultureType)
        );

        Add(new SimilarityFilter<TIndex, string>(
            CellLineFilterNames.Name,
            path.Join(specimen => specimen.CellLine.Name),
            criteria.Name)
        );


        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.MgmtStatus,
            path.Join(specimen => specimen.CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
            criteria.MgmtStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.IdhStatus,
            path.Join(specimen => specimen.CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
            criteria.IdhStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.IdhMutation,
            path.Join(specimen => specimen.CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
            criteria.IdhMutation)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.GeneExpressionSubtype,
            path.Join(specimen => specimen.CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
            criteria.GeneExpressionSubtype)
        );

        Add(new EqualityFilter<TIndex, object>(
            CellLineFilterNames.MethylationSubtype,
            path.Join(specimen => specimen.CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
            criteria.MethylationSubtype)
        );

        Add(new BooleanFilter<TIndex>(
            CellLineFilterNames.GcimpMethylation,
            path.Join(specimen => specimen.CellLine.MolecularData.GcimpMethylation),
            criteria.GcimpMethylation)
        );
    }
}
