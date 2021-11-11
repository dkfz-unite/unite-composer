using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters
{
    public class OrganoidCriteriaFiltersCollection : SpecimenCriteriaFiltersCollection
    {
        public OrganoidCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            _filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.OrganoidIndex>(
                SpecimenFilterNames.Type,
                specimen => specimen.Organoid)
            );


            if (criteria.OrganoidFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                    SpecimenFilterNames.Id,
                    specimen => specimen.Id,
                    criteria.OrganoidFilters.Id)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    OrganoidFilterNames.ReferenceId,
                    specimen => specimen.Organoid.ReferenceId,
                    criteria.OrganoidFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    OrganoidFilterNames.Medium,
                    specimen => specimen.Organoid.Medium,
                    criteria.OrganoidFilters.Medium)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    OrganoidFilterNames.Tumorigenicity,
                    specimen => specimen.Organoid.Tumorigenicity,
                    criteria.OrganoidFilters.Tumorigenicity)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    OrganoidFilterNames.Intervention,
                    specimen => specimen.Organoid.Interventions.First().Type,
                    criteria.OrganoidFilters.Intervention)
                );


                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    OrganoidFilterNames.MgmtStatus,
                    specimen => specimen.Organoid.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    OrganoidFilterNames.IdhStatus,
                    specimen => specimen.Organoid.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    OrganoidFilterNames.IdhMutation,
                    specimen => specimen.Organoid.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    OrganoidFilterNames.GeneExpressionSubtype,
                    specimen => specimen.Organoid.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    OrganoidFilterNames.MethylationSubtype,
                    specimen => specimen.Organoid.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    OrganoidFilterNames.GcimpMethylation,
                    specimen => specimen.Organoid.MolecularData.GcimpMethylation,
                    criteria.OrganoidFilters.GcimpMethylation)
                );
            }
        }
    }
}
