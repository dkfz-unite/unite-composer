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
    public MolecularDataFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, MolecularDataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.MgmtStatus))
        {
            Add(new EqualityFilter<TIndex, object>(
                SpecimenFilterNames.MgmtStatus,
                path.Join(molecularData => molecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus)
            );
        }

        if (IsNotEmpty(criteria.IdhStatus))
        {
            Add(new EqualityFilter<TIndex, object>(
                SpecimenFilterNames.IdhStatus,
                path.Join(molecularData => molecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus)
            );
        }

        if (IsNotEmpty(criteria.IdhMutation))
        {
            Add(new EqualityFilter<TIndex, object>(
                SpecimenFilterNames.IdhMutation,
                path.Join(molecularData => molecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation)
            );
        }

        if (IsNotEmpty(criteria.GeneExpressionSubtype))
        {
            Add(new EqualityFilter<TIndex, object>(
                SpecimenFilterNames.GeneExpressionSubtype,
                path.Join(molecularData => molecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype)
            );
        }

        if (IsNotEmpty(criteria.MethylationSubtype))
        {
            Add(new EqualityFilter<TIndex, object>(
                SpecimenFilterNames.MethylationSubtype,
                path.Join(molecularData => molecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype)
            );
        }

        if (IsNotEmpty(criteria.GcimpMethylation))
        {
            Add(new BooleanFilter<TIndex>(
                SpecimenFilterNames.GcimpMethylation,
                path.Join(molecularData => molecularData.GcimpMethylation),
                criteria.GcimpMethylation)
            );
        }
    }
}
