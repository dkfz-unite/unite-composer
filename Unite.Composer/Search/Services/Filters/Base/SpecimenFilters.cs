using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class SpecimenFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public SpecimenFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, SpecimenIndex>> path)
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

        //TODO: Add specimen type filter

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


        Add(new SimilarityFilter<TIndex, string>(
            SpecimenFilterNames.Drug,
            path.Join(specimen => specimen.DrugScreenings.First().Drug),
            criteria.Drug)
        );

        Add(new RangeFilter<TIndex, double?>(
            SpecimenFilterNames.Dss,
            path.Join(specimen => specimen.DrugScreenings.First().Dss),
            criteria.Dss.From,
            criteria.Dss.To)
        );

        Add(new RangeFilter<TIndex, double?>(
            SpecimenFilterNames.DssSelective,
            path.Join(specimen => specimen.DrugScreenings.First().DssSelective),
            criteria.DssSelective.From,
            criteria.DssSelective.To)
        );
    }
}
