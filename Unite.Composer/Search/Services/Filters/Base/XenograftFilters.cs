using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class XenograftFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class //FiltersCollection<TIndex> where TIndex : class
{
    public XenograftFilters(XenograftCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
    {
        //if (criteria == null)
        //{
        //    return;
        //}

        //Add(new EqualityFilter<TIndex, int>(
        //    SpecimenFilterNames.Id,
        //    path.Join(specimen => specimen.Id),
        //    criteria.Id)
        //);

        Add(new SimilarityFilter<TIndex, string>(
            XenograftFilterNames.ReferenceId,
            path.Join(specimen => specimen.Xenograft.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new SimilarityFilter<TIndex, string>(
            XenograftFilterNames.MouseStrain,
            path.Join(specimen => specimen.Xenograft.MouseStrain),
            criteria.MouseStrain)
        );

        Add(new BooleanFilter<TIndex>(
            XenograftFilterNames.Tumorigenicity,
            path.Join(specimen => specimen.Xenograft.Tumorigenicity),
            criteria.Tumorigenicity)
        );

        Add(new EqualityFilter<TIndex, object>(
            XenograftFilterNames.TumorGrowthForm,
            path.Join(specimen => specimen.Xenograft.TumorGrowthForm.Suffix(_keywordSuffix)),
            criteria.TumorGrowthForm)
        );

        Add(new MultiPropertyRangeFilter<TIndex, int?>(
            XenograftFilterNames.SurvivalDays,
            path.Join(specimen => specimen.Xenograft.SurvivalDaysFrom),
            path.Join(specimen => specimen.Xenograft.SurvivalDaysTo),
            criteria.SurvivalDays?.From,
            criteria.SurvivalDays?.To)
        );

        Add(new SimilarityFilter<TIndex, string>(
            XenograftFilterNames.Intervention,
            path.Join(specimen => specimen.Xenograft.Interventions.First().Type),
            criteria.Intervention)
        );


        //Add(new EqualityFilter<TIndex, object>(
        //    XenograftFilterNames.MgmtStatus,
        //    path.Join(specimen => specimen.Xenograft.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
        //    criteria.MgmtStatus)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    XenograftFilterNames.IdhStatus,
        //    path.Join(specimen => specimen.Xenograft.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
        //    criteria.IdhStatus)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    XenograftFilterNames.IdhMutation,
        //    path.Join(specimen => specimen.Xenograft.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
        //    criteria.IdhMutation)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    XenograftFilterNames.GeneExpressionSubtype,
        //    path.Join(specimen => specimen.Xenograft.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
        //    criteria.GeneExpressionSubtype)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    XenograftFilterNames.MethylationSubtype,
        //    path.Join(specimen => specimen.Xenograft.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
        //    criteria.MethylationSubtype)
        //);

        //Add(new BooleanFilter<TIndex>(
        //    XenograftFilterNames.GcimpMethylation,
        //    path.Join(specimen => specimen.Xenograft.MolecularData.GcimpMethylation),
        //    criteria.GcimpMethylation)
        //);


        //Add(new SimilarityFilter<TIndex, string>(
        //    XenograftFilterNames.Drug,
        //    path.Join(specimen => specimen.Xenograft.DrugScreenings.First().Drug),
        //    criteria.Drug)
        //);

        //Add(new RangeFilter<TIndex, double?>(
        //    XenograftFilterNames.Dss,
        //    path.Join(specimen => specimen.Xenograft.DrugScreenings.First().Dss),
        //    criteria.Dss.From,
        //    criteria.Dss.To)
        //);

        //Add(new RangeFilter<TIndex, double?>(
        //    XenograftFilterNames.DssSelective,
        //    path.Join(specimen => specimen.Xenograft.DrugScreenings.First().DssSelective),
        //    criteria.DssSelective.From,
        //    criteria.DssSelective.To)
        //);
    }
}
