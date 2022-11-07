using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class OrganoidFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class
{
    public OrganoidFilters(OrganoidCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new SimilarityFilter<TIndex, string>(
            OrganoidFilterNames.ReferenceId,
            path.Join(specimen => specimen.Organoid.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new SimilarityFilter<TIndex, string>(
            OrganoidFilterNames.Medium,
            path.Join(specimen => specimen.Organoid.Medium),
            criteria.Medium)
        );

        Add(new BooleanFilter<TIndex>(
            OrganoidFilterNames.Tumorigenicity,
            path.Join(specimen => specimen.Organoid.Tumorigenicity),
            criteria.Tumorigenicity)
        );

        Add(new SimilarityFilter<TIndex, string>(
            OrganoidFilterNames.Intervention,
            path.Join(specimen => specimen.Organoid.Interventions.First().Type),
            criteria.Intervention)
        );
    }
}
