using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class TissueFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class
{
    public TissueFilters(TissueCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        Add(new SimilarityFilter<TIndex, string>(
            TissueFilterNames.ReferenceId,
            path.Join(specimen => specimen.Tissue.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new EqualityFilter<TIndex, object>(
            TissueFilterNames.Type,
            path.Join(specimen => specimen.Tissue.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        Add(new EqualityFilter<TIndex, object>(
            TissueFilterNames.TumorType,
            path.Join(specimen => specimen.Tissue.TumorType.Suffix(_keywordSuffix)),
            criteria.TumorType)
        );

        Add(new SimilarityFilter<TIndex, string>(
            TissueFilterNames.Source,
            path.Join(specimen => specimen.Tissue.Source),
            criteria.Source)
        );
    }
}
