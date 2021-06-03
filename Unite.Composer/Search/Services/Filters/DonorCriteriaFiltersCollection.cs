using System.Collections.Generic;
using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Search.Services.Search
{
    public class DonorCriteriaFiltersCollection : CriteriaFiltersCollection<DonorIndex, SearchCriteria>
    {
        private const string _keywordSuffix = "keyword";


        public DonorCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
        }


        protected override IEnumerable<IFilter<DonorIndex>> MapCriteria(SearchCriteria criteria)
        {
            var filters = new List<IFilter<DonorIndex>>();

            if (criteria.DonorFilters != null)
            {
                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    DonorFilterNames.ReferenceId,
                    donor => donor.ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    DonorFilterNames.Diagnosis,
                    donor => donor.ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    DonorFilterNames.Gender,
                    donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                filters.Add(new RangeFilter<DonorIndex, int?>(
                    DonorFilterNames.Age,
                    donor => donor.ClinicalData.Age,
                    criteria.DonorFilters.Age.From,
                    criteria.DonorFilters.Age.To)
                );

                filters.Add(new BooleanFilter<DonorIndex>(
                    DonorFilterNames.VitalStatus,
                    donor => donor.ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );
            }

            if (criteria.TissueFilters != null)
            {
                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    TissueFilterNames.ReferenceId,
                    donor => donor.Mutations.First().Specimens.First().Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    TissueFilterNames.Type,
                    donor => donor.Mutations.First().Specimens.First().Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    TissueFilterNames.TumourType,
                    donor => donor.Mutations.First().Specimens.First().Tissue.TumourType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumourType)
                );

                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    TissueFilterNames.Source,
                    donor => donor.Mutations.First().Specimens.First().Tissue.Source,
                    criteria.TissueFilters.Source)
                );
            }

            if (criteria.CellLineFilters != null)
            {
                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    donor => donor.Mutations.First().Specimens.First().CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    CellLineFilterNames.Type,
                    donor => donor.Mutations.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    CellLineFilterNames.Species,
                    donor => donor.Mutations.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );
            }

            if (criteria.MutationFilters != null)
            {
                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    MutationFilterNames.Code,
                    donor => donor.Mutations.First().Code,
                    criteria.MutationFilters.Code)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Type,
                    donor => donor.Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Chromosome,
                    donor => donor.Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                filters.Add(new RangeFilter<DonorIndex, int?>(
                    MutationFilterNames.Position,
                    donor => donor.Mutations.First().Start,
                    criteria.MutationFilters.Position.From,
                    criteria.MutationFilters.Position.To)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Impact,
                    donor => donor.Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Consequence,
                    donor => donor.Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );

                filters.Add(new SimilarityFilter<DonorIndex, string>(
                    MutationFilterNames.Gene,
                    donor => donor.Mutations.First().AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene)
                );
            }

            return filters;
        }
    }
}
