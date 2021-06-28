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
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    DonorFilterNames.VitalStatus,
                    mutation => mutation.Donors.First().ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, object>(
                   DonorFilterNames.Therapy,
                   mutation => mutation.Donors.First().Treatments.First().Therapy,
                   criteria.DonorFilters.Therapy)
               );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    DonorFilterNames.MtaProtected,
                    mutation => mutation.Donors.First().MtaProtected,
                    criteria.DonorFilters.MtaProtected)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, object>(
                   DonorFilterNames.WorkPackage,
                   mutation => mutation.Donors.First().WorkPackages.First().Name.Suffix(_keywordSuffix),
                   criteria.DonorFilters.WorkPackage)
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
                    TissueFilterNames.TumorType,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.TumorType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumorType)
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
                    CellLineFilterNames.Species,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.Type,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.CultureType,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.CultureType.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.CultureType)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    CellLineFilterNames.Name,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Name,
                    criteria.CellLineFilters.Name)
                );

                AddSpecimenFilters(CellLineFilterNames.SpecimenFilterNames(), criteria.CellLineFilters);
            }

            if (criteria.OrganoidFilters != null)
            {
                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    OrganoidFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.ReferenceId,
                    criteria.OrganoidFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    OrganoidFilterNames.Medium,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.Medium,
                    criteria.OrganoidFilters.Medium)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    OrganoidFilterNames.Tumorigenicity,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.Tumorigenicity,
                    criteria.OrganoidFilters.Tumorigenicity)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    OrganoidFilterNames.Intervention,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.Interventions.First().Type,
                    criteria.OrganoidFilters.Intervention)
                );

                AddSpecimenFilters(OrganoidFilterNames.SpecimenFilterNames(), criteria.OrganoidFilters);
            }

            if (criteria.XenograftFilters != null)
            {
                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    XenograftFilterNames.ReferenceId,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.ReferenceId,
                    criteria.XenograftFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    XenograftFilterNames.MouseStrain,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MouseStrain,
                    criteria.XenograftFilters.MouseStrain)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    XenograftFilterNames.Tumorigenicity,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.Tumorigenicity,
                    criteria.XenograftFilters.Tumorigenicity)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    XenograftFilterNames.TumorGrowthForm,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.TumorGrowthForm.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.TumorGrowthForm)
                );

                _filters.Add(new MultiPropertyRangeFilter<MutationIndex, double?>(
                    XenograftFilterNames.SurvivalDays,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.SurvivalDaysFrom,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.SurvivalDaysTo,
                    criteria.XenograftFilters.SurvivalDays?.From,
                    criteria.XenograftFilters.SurvivalDays?.To)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    XenograftFilterNames.Intervention,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.Interventions.First().Type,
                    criteria.XenograftFilters.Intervention)
                );

                AddSpecimenFilters(XenograftFilterNames.SpecimenFilterNames(), criteria.XenograftFilters);
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

                _filters.Add(new MultiPropertyRangeFilter<MutationIndex, int?>(
                    MutationFilterNames.Position,
                    mutation => mutation.Start,
                    mutation => mutation.End,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To)
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
                filterNames.MgmtStatus,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                criteria.MgmtStatus)
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
               filterNames.GeneExpressionSubtype,
               mutation => mutation.Donors.First().Specimens.First().MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
               criteria.GeneExpressionSubtype)
           );

            _filters.Add(new EqualityFilter<MutationIndex, object>(
                filterNames.MethylationSubtype,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                criteria.MethylationSubtype)
            );

            _filters.Add(new BooleanFilter<MutationIndex>(
                filterNames.GcimpMethylation,
                mutation => mutation.Donors.First().Specimens.First().MolecularData.GcimpMethylation,
                criteria.GcimpMethylation)
            );
        }
    }
}
