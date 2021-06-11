using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Search.Services.Search
{
    public class MutationCriteriaFiltersCollection : CriteriaFiltersCollection<MutationIndex>
    {
        private const string _keywordSuffix = "keyword";


        public MutationCriteriaFiltersCollection(SearchCriteria criteria) : base()
        {
            if (criteria.DonorFilters != null)
            {
                _filters.Add(new EqualityFilter<MutationIndex, int>(
                    DonorFilterNames.Id,
                    mutation => mutation.Donors.First().Id,
                    criteria.DonorFilters.Id)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    DonorFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    DonorFilterNames.Diagnosis,
                    mutation => mutation.Donors.First().ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    DonorFilterNames.Gender,
                    mutation => mutation.Donors.First().ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                _filters.Add(new RangeFilter<MutationIndex, int?>(
                    DonorFilterNames.Age,
                    mutation => mutation.Donors.First().ClinicalData.Age,
                    criteria.DonorFilters.Age.From,
                    criteria.DonorFilters.Age.To)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    DonorFilterNames.VitalStatus,
                    mutation => mutation.Donors.First().ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );
            }

            if (criteria.TissueFilters != null)
            {
                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    TissueFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.Type,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.TumourType,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.TumourType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumourType)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    TissueFilterNames.Source,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.Source,
                    criteria.TissueFilters.Source)
                );

                AddSpecimenFilters(TissueFilterNames.SpecimenFilterNames(), criteria.TissueFilters);
            }

            if (criteria.CellLineFilters != null)
            {
                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.Type,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.Species,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    CellLineFilterNames.Name,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Name,
                    criteria.CellLineFilters.Name)
                );

                AddSpecimenFilters(CellLineFilterNames.SpecimenFilterNames(), criteria.CellLineFilters);
            }

            if (criteria.MutationFilters != null)
            {
                _filters.Add(new EqualityFilter<MutationIndex, long>(
                    MutationFilterNames.Id,
                    mutation => mutation.Id,
                    criteria.MutationFilters.Id)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    MutationFilterNames.Code,
                    mutation => mutation.Code,
                    criteria.MutationFilters.Code)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Type,
                    mutation => mutation.Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Chromosome,
                    mutation => mutation.Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                _filters.Add(new RangeFilter<MutationIndex, int?>(
                    MutationFilterNames.Position,
                    mutation => mutation.Start,
                    criteria.MutationFilters.Position.From,
                    criteria.MutationFilters.Position.To)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Impact,
                    mutation => mutation.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    MutationFilterNames.Consequence,
                    mutation => mutation.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    MutationFilterNames.Gene,
                    mutation => mutation.AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene)
                );
            }
        }

        private void AddSpecimenFilters(SpecimenFilterNames filterNames, SpecimenCriteria criteria)
        {
            _filters.Add(new EqualityFilter<MutationIndex, int>(
                filterNames.Id,
                mutation => mutation.Donors.First().Specimens.First().Id,
                criteria.Id)
            );

            _filters.Add(new EqualityFilter<MutationIndex, object>(
                filterNames.GeneExpressionSubtype,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<MutationIndex, object>(
                filterNames.IdhStatus,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.IdhStatus.Suffix(_keywordSuffix),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<MutationIndex, object>(
                filterNames.IdhMutation,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.IdhMutation.Suffix(_keywordSuffix),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<MutationIndex, object>(
                filterNames.MethylationStatus,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.MethylationStatus.Suffix(_keywordSuffix),
                criteria.MethylationStatus)
            );

            _filters.Add(new EqualityFilter<MutationIndex, object>(
                filterNames.MethylationType,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.MethylationType.Suffix(_keywordSuffix),
                criteria.MethylationType)
            );

            _filters.Add(new BooleanFilter<MutationIndex>(
                filterNames.GcimpMethylation,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.GcimpMethylation,
                criteria.GcimpMethylation)
            );
        }
    }
}
