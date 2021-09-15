using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services
{
    public class SpecimenCriteriaFiltersCollection : CriteriaFiltersCollection<SpecimenIndex>
    {
        protected const string _keywordSuffix = "keyword";


        public SpecimenCriteriaFiltersCollection(SearchCriteria criteria) : base()
        {
            if (criteria.DonorFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                    DonorFilterNames.Id,
                    specimen => specimen.Donor.Id,
                    criteria.DonorFilters.Id)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    DonorFilterNames.ReferenceId,
                    specimen => specimen.Donor.ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    DonorFilterNames.Diagnosis,
                    specimen => specimen.Donor.ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    DonorFilterNames.Gender,
                    specimen => specimen.Donor.ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                _filters.Add(new RangeFilter<SpecimenIndex, int?>(
                    DonorFilterNames.Age,
                    specimen => specimen.Donor.ClinicalData.Age,
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    DonorFilterNames.VitalStatus,
                    specimen => specimen.Donor.ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, object>(
                   DonorFilterNames.Therapy,
                   specimen => specimen.Donor.Treatments.First().Therapy,
                   criteria.DonorFilters.Therapy)
               );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    DonorFilterNames.MtaProtected,
                    specimen => specimen.Donor.MtaProtected,
                    criteria.DonorFilters.MtaProtected)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, object>(
                   DonorFilterNames.WorkPackage,
                   specimen => specimen.Donor.WorkPackages.First().Name.Suffix(_keywordSuffix),
                   criteria.DonorFilters.WorkPackage)
               );
            }

            if (criteria.SpecimenFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                    SpecimenFilterNames.Id,
                    specimen => specimen.Id,
                    criteria.SpecimenFilters.Id)
                );
            }

            if (criteria.GeneFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                    GeneFilterNames.Id,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene.Id,
                    criteria.GeneFilters.Id)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    GeneFilterNames.Symbol,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene.Symbol,
                    criteria.GeneFilters.Symbol)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    GeneFilterNames.Biotype,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene.Biotype.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Biotype)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    GeneFilterNames.Chromosome,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene.Chromosome.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Chromosome)
                );

                _filters.Add(new MultiPropertyRangeFilter<SpecimenIndex, int?>(
                    GeneFilterNames.Position,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene.Start,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene.End,
                    criteria.GeneFilters.Position?.From,
                    criteria.GeneFilters.Position?.To)
                );
            }

            if (criteria.MutationFilters != null)
            {
                _filters.Add(new EqualityFilter<SpecimenIndex, long>(
                    MutationFilterNames.Id,
                    specimen => specimen.Mutations.First().Id,
                    criteria.MutationFilters.Id)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    MutationFilterNames.Code,
                    specimen => specimen.Mutations.First().Code,
                    criteria.MutationFilters.Code)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Type,
                    specimen => specimen.Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Chromosome,
                    specimen => specimen.Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                _filters.Add(new MultiPropertyRangeFilter<SpecimenIndex, int?>(
                    MutationFilterNames.Position,
                    specimen => specimen.Mutations.First().Start,
                    specimen => specimen.Mutations.First().End,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Impact,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    MutationFilterNames.Consequence,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );
            }
        }
    }
}
