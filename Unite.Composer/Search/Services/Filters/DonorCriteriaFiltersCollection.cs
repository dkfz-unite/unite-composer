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
            if (criteria.DonorFilters != null)
            {
                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    DonorFilterNames.ReferenceId,
                    donor => donor.ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    DonorFilterNames.Diagnosis,
                    donor => donor.ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    DonorFilterNames.Gender,
                    donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                _filters.Add(new RangeFilter<DonorIndex, int?>(
                    DonorFilterNames.Age,
                    donor => donor.ClinicalData.Age,
                    criteria.DonorFilters.Age.From,
                    criteria.DonorFilters.Age.To)
                );

                _filters.Add(new BooleanFilter<DonorIndex>(
                    DonorFilterNames.VitalStatus,
                    donor => donor.ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );
            }

            if (criteria.TissueFilters != null)
            {
                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    TissueFilterNames.ReferenceId,
                    donor => donor.Mutations.First().Specimens.First().Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    TissueFilterNames.Type,
                    donor => donor.Mutations.First().Specimens.First().Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    TissueFilterNames.TumourType,
                    donor => donor.Mutations.First().Specimens.First().Tissue.TumourType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumourType)
                );

                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    TissueFilterNames.Source,
                    donor => donor.Mutations.First().Specimens.First().Tissue.Source,
                    criteria.TissueFilters.Source)
                );

                AddSpecimenFilters(TissueFilterNames.SpecimenFilterNames(), criteria.TissueFilters);
            }

            if (criteria.CellLineFilters != null)
            {
                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    donor => donor.Mutations.First().Specimens.First().CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    CellLineFilterNames.Type,
                    donor => donor.Mutations.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    CellLineFilterNames.Species,
                    donor => donor.Mutations.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );

                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                   CellLineFilterNames.Name,
                   donor => donor.Mutations.First().Specimens.First().CellLine.Name,
                   criteria.CellLineFilters.Name)
               );

                AddSpecimenFilters(CellLineFilterNames.SpecimenFilterNames(), criteria.CellLineFilters);
            }

            if (criteria.MutationFilters != null)
            {
                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    MutationFilterNames.Code,
                    donor => donor.Mutations.First().Code,
                    criteria.MutationFilters.Code)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Type,
                    donor => donor.Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Chromosome,
                    donor => donor.Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                _filters.Add(new RangeFilter<DonorIndex, int?>(
                    MutationFilterNames.Position,
                    donor => donor.Mutations.First().Start,
                    criteria.MutationFilters.Position.From,
                    criteria.MutationFilters.Position.To)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Impact,
                    donor => donor.Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                _filters.Add(new EqualityFilter<DonorIndex, object>(
                    MutationFilterNames.Consequence,
                    donor => donor.Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );

                _filters.Add(new SimilarityFilter<DonorIndex, string>(
                    MutationFilterNames.Gene,
                    donor => donor.Mutations.First().AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene)
                );
            }
        }


        private void AddSpecimenFilters(SpecimenFilterNames filterNames, SpecimenCriteria criteria)
        {
            _filters.Add(new EqualityFilter<DonorIndex, object>(
                filterNames.GeneExpressionSubtype,
                donor => donor.Mutations.First().Specimens.First().MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<DonorIndex, object>(
                filterNames.IdhStatus,
                donor => donor.Mutations.First().Specimens.First().MolecularData.IdhStatus.Suffix(_keywordSuffix),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<DonorIndex, object>(
                filterNames.IdhMutation,
                donor => donor.Mutations.First().Specimens.First().MolecularData.IdhMutation.Suffix(_keywordSuffix),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<DonorIndex, object>(
                filterNames.MethylationStatus,
                donor => donor.Mutations.First().Specimens.First().MolecularData.MethylationStatus.Suffix(_keywordSuffix),
                criteria.MethylationStatus)
            );

            _filters.Add(new EqualityFilter<DonorIndex, object>(
                filterNames.MethylationType,
                donor => donor.Mutations.First().Specimens.First().MolecularData.MethylationType.Suffix(_keywordSuffix),
                criteria.MethylationType)
            );

            _filters.Add(new BooleanFilter<DonorIndex>(
                filterNames.GcimpMethylation,
                donor => donor.Mutations.First().Specimens.First().MolecularData.GcimpMethylation,
                criteria.GcimpMethylation)
            );
        }
    }
}
