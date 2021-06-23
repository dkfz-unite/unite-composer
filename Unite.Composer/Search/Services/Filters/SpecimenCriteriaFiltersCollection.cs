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

            if (criteria.TissueFilters != null)
            {
                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    TissueFilterNames.ReferenceId,
                    specimen => specimen.Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    TissueFilterNames.Type,
                    specimen => specimen.Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    TissueFilterNames.TumorType,
                    specimen => specimen.Tissue.TumorType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumorType)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    TissueFilterNames.Source,
                    specimen => specimen.Tissue.Source,
                    criteria.TissueFilters.Source)
                );

                AddSpecimenFilters(TissueFilterNames.SpecimenFilterNames(), criteria.TissueFilters);
            }

            if (criteria.CellLineFilters != null)
            {
                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    specimen => specimen.CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.Species,
                    specimen => specimen.CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.Type,
                    specimen => specimen.CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    CellLineFilterNames.CultureType,
                    specimen => specimen.CellLine.CultureType.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.CultureType)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    CellLineFilterNames.Name,
                    specimen => specimen.CellLine.Name,
                    criteria.CellLineFilters.Name)
                );

                AddSpecimenFilters(CellLineFilterNames.SpecimenFilterNames(), criteria.CellLineFilters);
            }

            if (criteria.OrganoidFilters != null)
            {
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

                AddSpecimenFilters(OrganoidFilterNames.SpecimenFilterNames(), criteria.OrganoidFilters);
            }

            if (criteria.XenograftFilters != null)
            {
                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    XenograftFilterNames.ReferenceId,
                    specimen => specimen.Xenograft.ReferenceId,
                    criteria.XenograftFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    XenograftFilterNames.MouseStrain,
                    specimen => specimen.Xenograft.MouseStrain,
                    criteria.XenograftFilters.MouseStrain)
                );

                _filters.Add(new BooleanFilter<SpecimenIndex>(
                    XenograftFilterNames.Tumorigenicity,
                    specimen => specimen.Xenograft.Tumorigenicity,
                    criteria.XenograftFilters.Tumorigenicity)
                );

                _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                    XenograftFilterNames.TumorGrowthForm,
                    specimen => specimen.Xenograft.TumorGrowthForm.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.TumorGrowthForm)
                );

                _filters.Add(new MultiPropertyRangeFilter<SpecimenIndex, double?>(
                    XenograftFilterNames.SurvivalDays,
                    specimen => specimen.Xenograft.SurvivalDaysFrom,
                    specimen => specimen.Xenograft.SurvivalDaysTo,
                    criteria.XenograftFilters.SurvivalDays?.From,
                    criteria.XenograftFilters.SurvivalDays?.To)
                );

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    XenograftFilterNames.Intervention,
                    specimen => specimen.Xenograft.Interventions.First().Type,
                    criteria.XenograftFilters.Intervention)
                );

                AddSpecimenFilters(XenograftFilterNames.SpecimenFilterNames(), criteria.OrganoidFilters);
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

                _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
                    MutationFilterNames.Gene,
                    specimen => specimen.Mutations.First().AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene)
                );
            }
        }


        private void AddSpecimenFilters(SpecimenFilterNames filterNames, SpecimenCriteria criteria)
        {
            _filters.Add(new EqualityFilter<SpecimenIndex, int>(
                filterNames.Id,
                specimen => specimen.Id,
                criteria.Id)
            );

            _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                filterNames.MgmtStatus,
                specimen => specimen.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                criteria.MgmtStatus)
            );

            _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                filterNames.IdhStatus,
                specimen => specimen.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                criteria.IdhStatus)
            );

            _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                filterNames.IdhMutation,
                specimen => specimen.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                criteria.IdhMutation)
            );

            _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                filterNames.GeneExpressionSubtype,
                specimen => specimen.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                criteria.GeneExpressionSubtype)
            );

            _filters.Add(new EqualityFilter<SpecimenIndex, object>(
                filterNames.MethylationSubtype,
                specimen => specimen.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                criteria.MethylationSubtype)
            );

            _filters.Add(new BooleanFilter<SpecimenIndex>(
                filterNames.GcimpMethylation,
                specimen => specimen.MolecularData.GcimpMethylation,
                criteria.GcimpMethylation)
            );
        }
    }
}
