using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class MolecularDataFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public MolecularDataFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, TissueIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MgmtStatus,
            path.Join(specimen => specimen.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
            criteria.MgmtStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhStatus,
            path.Join(specimen => specimen.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
            criteria.IdhStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhMutation,
            path.Join(specimen => specimen.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
            criteria.IdhMutation)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.GeneExpressionSubtype,
            path.Join(specimen => specimen.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
            criteria.GeneExpressionSubtype)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MethylationSubtype,
            path.Join(specimen => specimen.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
            criteria.MethylationSubtype)
        );

        Add(new BooleanFilter<TIndex>(
            SpecimenFilterNames.GcimpMethylation,
            path.Join(specimen => specimen.MolecularData.GcimpMethylation),
            criteria.GcimpMethylation)
        );
    }

    public MolecularDataFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, CellLineIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MgmtStatus,
            path.Join(specimen => specimen.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
            criteria.MgmtStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhStatus,
            path.Join(specimen => specimen.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
            criteria.IdhStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhMutation,
            path.Join(specimen => specimen.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
            criteria.IdhMutation)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.GeneExpressionSubtype,
            path.Join(specimen => specimen.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
            criteria.GeneExpressionSubtype)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MethylationSubtype,
            path.Join(specimen => specimen.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
            criteria.MethylationSubtype)
        );

        Add(new BooleanFilter<TIndex>(
            SpecimenFilterNames.GcimpMethylation,
            path.Join(specimen => specimen.MolecularData.GcimpMethylation),
            criteria.GcimpMethylation)
        );
    }

    public MolecularDataFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, OrganoidIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MgmtStatus,
            path.Join(specimen => specimen.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
            criteria.MgmtStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhStatus,
            path.Join(specimen => specimen.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
            criteria.IdhStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhMutation,
            path.Join(specimen => specimen.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
            criteria.IdhMutation)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.GeneExpressionSubtype,
            path.Join(specimen => specimen.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
            criteria.GeneExpressionSubtype)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MethylationSubtype,
            path.Join(specimen => specimen.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
            criteria.MethylationSubtype)
        );

        Add(new BooleanFilter<TIndex>(
            SpecimenFilterNames.GcimpMethylation,
            path.Join(specimen => specimen.MolecularData.GcimpMethylation),
            criteria.GcimpMethylation)
        );
    }

    public MolecularDataFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, XenograftIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MgmtStatus,
            path.Join(specimen => specimen.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
            criteria.MgmtStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhStatus,
            path.Join(specimen => specimen.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
            criteria.IdhStatus)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.IdhMutation,
            path.Join(specimen => specimen.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
            criteria.IdhMutation)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.GeneExpressionSubtype,
            path.Join(specimen => specimen.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
            criteria.GeneExpressionSubtype)
        );

        Add(new EqualityFilter<TIndex, object>(
            SpecimenFilterNames.MethylationSubtype,
            path.Join(specimen => specimen.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
            criteria.MethylationSubtype)
        );

        Add(new BooleanFilter<TIndex>(
            SpecimenFilterNames.GcimpMethylation,
            path.Join(specimen => specimen.MolecularData.GcimpMethylation),
            criteria.GcimpMethylation)
        );
    }
}
