using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Search.Services.Filters
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

            if (criteria.SpecimenFilters != null)
            {
                _filters.Add(new EqualityFilter<MutationIndex, int>(
                    SpecimenFilterNames.Id,
                    mutation => mutation.Donors.First().Specimens.First().Id,
                    criteria.SpecimenFilters.Id)
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


                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.MgmtStatus,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.TissueFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.IdhStatus,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.TissueFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.IdhMutation,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.TissueFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.GeneExpressionSubtype,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.TissueFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    TissueFilterNames.MethylationSubtype,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.TissueFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    TissueFilterNames.GcimpMethylation,
                    mutation => mutation.Donors.First().Specimens.First().Tissue.MolecularData.GcimpMethylation,
                    criteria.TissueFilters.GcimpMethylation)
                );
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


                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.MgmtStatus,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.IdhStatus,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.IdhMutation,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.GeneExpressionSubtype,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    CellLineFilterNames.MethylationSubtype,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    CellLineFilterNames.GcimpMethylation,
                    mutation => mutation.Donors.First().Specimens.First().CellLine.MolecularData.GcimpMethylation,
                    criteria.CellLineFilters.GcimpMethylation)
                );
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


                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    OrganoidFilterNames.MgmtStatus,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    OrganoidFilterNames.IdhStatus,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    OrganoidFilterNames.IdhMutation,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    OrganoidFilterNames.GeneExpressionSubtype,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    OrganoidFilterNames.MethylationSubtype,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    OrganoidFilterNames.GcimpMethylation,
                    mutation => mutation.Donors.First().Specimens.First().Organoid.MolecularData.GcimpMethylation,
                    criteria.OrganoidFilters.GcimpMethylation)
                );
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


                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    XenograftFilterNames.MgmtStatus,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    XenograftFilterNames.IdhStatus,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    XenograftFilterNames.IdhMutation,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    XenograftFilterNames.GeneExpressionSubtype,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    XenograftFilterNames.MethylationSubtype,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<MutationIndex>(
                    XenograftFilterNames.GcimpMethylation,
                    mutation => mutation.Donors.First().Specimens.First().Xenograft.MolecularData.GcimpMethylation,
                    criteria.XenograftFilters.GcimpMethylation)
                );
            }

            if (criteria.GeneFilters != null)
            {
                _filters.Add(new EqualityFilter<MutationIndex, int>(
                    GeneFilterNames.Id,
                    mutation => mutation.AffectedTranscripts.First().Transcript.Gene.Id,
                    criteria.GeneFilters.Id)
                );

                _filters.Add(new SimilarityFilter<MutationIndex, string>(
                    GeneFilterNames.Symbol,
                    mutation => mutation.AffectedTranscripts.First().Transcript.Gene.Symbol,
                    criteria.GeneFilters.Symbol)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    GeneFilterNames.Biotype,
                    mutation => mutation.AffectedTranscripts.First().Transcript.Gene.Biotype.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Biotype)
                );

                _filters.Add(new EqualityFilter<MutationIndex, object>(
                    GeneFilterNames.Chromosome,
                    mutation => mutation.AffectedTranscripts.First().Transcript.Gene.Chromosome.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Chromosome)
                );

                _filters.Add(new MultiPropertyRangeFilter<MutationIndex, int?>(
                    GeneFilterNames.Position,
                    mutation => mutation.AffectedTranscripts.First().Transcript.Gene.Start,
                    mutation => mutation.AffectedTranscripts.First().Transcript.Gene.End,
                    criteria.GeneFilters.Position?.From,
                    criteria.GeneFilters.Position?.To)
                );
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
            }
        }
    }
}
