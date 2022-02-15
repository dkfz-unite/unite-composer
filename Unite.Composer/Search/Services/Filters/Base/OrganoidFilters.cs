using System;
using System.Linq;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base
{
    public class OrganoidFilters<TIndex> : FiltersCollection<TIndex>
        where TIndex : class
    {
        public OrganoidFilters(OrganoidCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path)
        {
            if (criteria == null)
            {
                return;
            }

            _filters.Add(new NotNullFilter<TIndex, OrganoidIndex>(
                SpecimenFilterNames.Type,
                path.Join(specimen => specimen.Organoid))
            );

            _filters.Add(new EqualityFilter<TIndex, int>(
                SpecimenFilterNames.Id,
                path.Join(specimen => specimen.Id),
                criteria.Id)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                OrganoidFilterNames.ReferenceId,
                path.Join(specimen => specimen.Organoid.ReferenceId),
                criteria.ReferenceId)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                OrganoidFilterNames.Medium,
                path.Join(specimen => specimen.Organoid.Medium),
                criteria.Medium)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                OrganoidFilterNames.Tumorigenicity,
                path.Join(specimen => specimen.Organoid.Tumorigenicity),
                criteria.Tumorigenicity)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                OrganoidFilterNames.Intervention,
                path.Join(specimen => specimen.Organoid.Interventions.First().Type),
                criteria.Intervention)
            );


            _filters.Add(new EqualityFilter<TIndex, object>(
                OrganoidFilterNames.MgmtStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                OrganoidFilterNames.IdhStatus,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                OrganoidFilterNames.IdhMutation,
                path.Join(specimen => specimen.CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                OrganoidFilterNames.GeneExpressionSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                OrganoidFilterNames.MethylationSubtype,
                path.Join(specimen => specimen.CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                OrganoidFilterNames.GcimpMethylation,
                path.Join(specimen => specimen.CellLine.MolecularData.GcimpMethylation),
                criteria.GcimpMethylation)
            );
        }
    }
}
