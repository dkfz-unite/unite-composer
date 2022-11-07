using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class XenograftFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class
{
    public XenograftFilters(XenograftCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

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
    }
}
