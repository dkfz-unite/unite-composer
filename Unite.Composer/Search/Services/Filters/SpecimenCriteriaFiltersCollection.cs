using System.Collections.Generic;
using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Search
{
    public class SpecimenCriteriaFiltersCollection : CriteriaFiltersCollection<SpecimenIndex, SearchCriteria>
    {
        private const string _keywordSuffix = "keyword";


        public SpecimenCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
        }


        protected override IEnumerable<IFilter<SpecimenIndex>> MapCriteria(SearchCriteria criteria)
        {
            var filters = new List<IFilter<SpecimenIndex>>();

            if (criteria.DonorFilters != null)
            {
                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    DonorFilterNames.ReferenceId,
                    specimen => specimen.Donor.ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    DonorFilterNames.Diagnosis,
                    specimen => specimen.Donor.ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    DonorFilterNames.Gender,
                    specimen => specimen.Donor.ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                filters.Add(new RangeFilter<SpecimenIndex, int?>(
                    DonorFilterNames.Age,
                    specimen => specimen.Donor.ClinicalData.Age,
                    criteria.DonorFilters.Age.From,
                    criteria.DonorFilters.Age.To)
                );

                filters.Add(new BooleanFilter<SpecimenIndex>(
                    DonorFilterNames.VitalStatus,
                    specimen => specimen.Donor.ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );
            }

            if (criteria.TissueFilters != null)
            {
                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    TissueFilterNames.ReferenceId,
                    specimen => specimen.Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    TissueFilterNames.Type,
                    specimen => specimen.Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    TissueFilterNames.TumourType,
                    specimen => specimen.Tissue.TumourType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumourType)
                );

                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    TissueFilterNames.Source,
                    specimen => specimen.Tissue.Source,
                    criteria.TissueFilters.Source)
                );
            }

            if (criteria.CellLineFilters != null)
            {
                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    specimen => specimen.CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.Type,
                    specimen => specimen.CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.Species,
                    specimen => specimen.CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );
            }

            if (criteria.MutationFilters != null)
            {
                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    MutationFilterNames.Code,
                    specimen => specimen.Mutations.First().Code,
                    criteria.MutationFilters.Code)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Type,
                    specimen => specimen.Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Chromosome,
                    specimen => specimen.Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                filters.Add(new RangeFilter<SpecimenIndex, int?>(
                    MutationFilterNames.Position,
                    specimen => specimen.Mutations.First().Start,
                    criteria.MutationFilters.Position.From,
                    criteria.MutationFilters.Position.To)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Impact,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Consequence,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );

                filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    MutationFilterNames.Gene,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene)
                );
            }

            return filters;
        }
    }
}
