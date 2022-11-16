using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class DrugScreeningFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public DrugScreeningFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, CellLineIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

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

    public DrugScreeningFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, OrganoidIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

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

    public DrugScreeningFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, XenograftIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

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
