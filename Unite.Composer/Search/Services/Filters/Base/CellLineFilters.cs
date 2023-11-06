using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class CellLineFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class
{
    public CellLineFilters(CellLineCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<TIndex, string>(
                CellLineFilterNames.ReferenceId,
                path.Join(specimen => specimen.Cell.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.Species))
        {
            Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.Species,
                path.Join(specimen => specimen.Cell.Species.Suffix(_keywordSuffix)),
                criteria.Species)
            );
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.Type,
                path.Join(specimen => specimen.Cell.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );
        }

        if (IsNotEmpty(criteria.CultureType))
        {
            Add(new EqualityFilter<TIndex, object>(
                CellLineFilterNames.CultureType,
                path.Join(specimen => specimen.Cell.CultureType.Suffix(_keywordSuffix)),
                criteria.CultureType)
            );
        }

        if (IsNotEmpty(criteria.Name))
        {
            Add(new SimilarityFilter<TIndex, string>(
                CellLineFilterNames.Name,
                path.Join(specimen => specimen.Cell.Name),
                criteria.Name)
            );
        }
    }
}
