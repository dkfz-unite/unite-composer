using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class DrugScreeningFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public DrugScreeningFilters(SpecimenCriteriaBase criteria, Expression<Func<TIndex, DrugScreeningIndex[]>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Drug))
        {
            Add(new SimilarityFilter<TIndex, string>(
                SpecimenFilterNames.Drug,
                path.Join(screenings => screenings.First().Drug),
                criteria.Drug)
            );
        }

        if (IsNotEmpty(criteria.Dss))
        {
            Add(new RangeFilter<TIndex, double?>(
                SpecimenFilterNames.Dss,
                path.Join(screenings => screenings.First().Dss),
                criteria.Dss.From,
                criteria.Dss.To)
            );
        }

        if (IsNotEmpty(criteria.DssSelective))
        {
            Add(new RangeFilter<TIndex, double?>(
                SpecimenFilterNames.DssSelective,
                path.Join(screenings => screenings.First().DssSelective),
                criteria.DssSelective.From,
                criteria.DssSelective.To)
            );
        }
    }
}
