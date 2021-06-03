using System.Collections.Generic;
using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Search.Services.Search
{
    public class MutationCriteriaFiltersCollection : CriteriaFiltersCollection<MutationIndex, SearchCriteria>
    {
        private const string _keywordSuffix = "keyword";


        public MutationCriteriaFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
        }


        protected override IEnumerable<IFilter<MutationIndex>> MapCriteria(SearchCriteria criteria)
        {
            var filters = new List<IFilter<MutationIndex>>();

            if (criteria.DonorFilters != null)
            {
                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    DonorFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    DonorFilterNames.Diagnosis,
                    mutation => mutation.Donors.First().ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    DonorFilterNames.Gender,
                    mutation => mutation.Donors.First().ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                filters.Add(new RangeFilter<MutationIndex, int?>(
                    DonorFilterNames.Age,
                    mutation => mutation.Donors.First().ClinicalData.Age,
                    criteria.DonorFilters.Age.From,
                    criteria.DonorFilters.Age.To)
                );

                filters.Add(new BooleanFilter<MutationIndex>(
                    DonorFilterNames.VitalStatus,
                    mutation => mutation.Donors.First().ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );
            }

            if (criteria.TissueFilters != null)
            {
                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    TissueFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.Type,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.TumourType,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.TumourType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumourType)
                );

                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    TissueFilterNames.Source,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.Source,
                    criteria.TissueFilters.Source)
                );
            }

            if (criteria.CellLineFilters != null)
            {
                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.Type,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.Species,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );
            }

            if (criteria.MutationFilters != null)
            {
                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    MutationFilterNames.Code,
                    mutation => mutation.Code,
                    criteria.MutationFilters.Code)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Type,
                    mutation => mutation.Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Chromosome,
                    mutation => mutation.Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                filters.Add(new RangeFilter<MutationIndex, int?>(
                    MutationFilterNames.Position,
                    mutation => mutation.Start,
                    criteria.MutationFilters.Position.From,
                    criteria.MutationFilters.Position.To)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Impact,
                    mutation => mutation.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Consequence,
                    mutation => mutation.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );

                filters.Add(new SimilarityFilter<MutationIndex, string>(
                    MutationFilterNames.Gene,
                    mutation => mutation.AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene)
                );
            }

            return filters;
        }
    }
}
