using System.Linq;
using Nest;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Search.Services.Filters
{
    public class GeneCriteriaFiltersCollection : CriteriaFiltersCollection<GeneIndex>
    {
        private const string _keywordSuffix = "keyword";


        public GeneCriteriaFiltersCollection(SearchCriteria criteria) : base()
        {
            if (criteria.DonorFilters != null)
            {
                _filters.Add(new EqualityFilter<GeneIndex, int>(
                    DonorFilterNames.Id,
                    gene => gene.Mutations.First().Donors.First().Id,
                    criteria.DonorFilters.Id)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    DonorFilterNames.ReferenceId,
                    gene => gene.Mutations.First().Donors.First().ReferenceId,
                    criteria.DonorFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    DonorFilterNames.Diagnosis,
                    gene => gene.Mutations.First().Donors.First().ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    DonorFilterNames.Gender,
                    gene => gene.Mutations.First().Donors.First().ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender)
                );

                _filters.Add(new RangeFilter<GeneIndex, int?>(
                    DonorFilterNames.Age,
                    gene => gene.Mutations.First().Donors.First().ClinicalData.Age,
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    DonorFilterNames.VitalStatus,
                    gene => gene.Mutations.First().Donors.First().ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, object>(
                   DonorFilterNames.Therapy,
                   gene => gene.Mutations.First().Donors.First().Treatments.First().Therapy,
                   criteria.DonorFilters.Therapy)
               );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    DonorFilterNames.MtaProtected,
                    gene => gene.Mutations.First().Donors.First().MtaProtected,
                    criteria.DonorFilters.MtaProtected)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, object>(
                   DonorFilterNames.WorkPackage,
                   gene => gene.Mutations.First().Donors.First().WorkPackages.First().Name.Suffix(_keywordSuffix),
                   criteria.DonorFilters.WorkPackage)
               );
            }

            if (criteria.SpecimenFilters != null)
            {
                _filters.Add(new EqualityFilter<GeneIndex, int>(
                    SpecimenFilterNames.Id,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Id,
                    criteria.SpecimenFilters.Id)
                );
            }

            if (criteria.TissueFilters != null)
            {
                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    TissueFilterNames.ReferenceId,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.ReferenceId,
                    criteria.TissueFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.Type,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.Type.Suffix(_keywordSuffix),
                    criteria.TissueFilters.Type)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.TumorType,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.TumorType.Suffix(_keywordSuffix),
                    criteria.TissueFilters.TumorType)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    TissueFilterNames.Source,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.Source,
                    criteria.TissueFilters.Source)
                );


                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.MgmtStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.TissueFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.IdhStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.TissueFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.IdhMutation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.TissueFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.GeneExpressionSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.TissueFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    TissueFilterNames.MethylationSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.TissueFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    TissueFilterNames.GcimpMethylation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Tissue.MolecularData.GcimpMethylation,
                    criteria.TissueFilters.GcimpMethylation)
                );
            }

            if (criteria.CellLineFilters != null)
            {
                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    CellLineFilterNames.ReferenceId,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.ReferenceId,
                    criteria.CellLineFilters.ReferenceId)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.Species,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.Type,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.CultureType,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.CultureType.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.CultureType)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                   CellLineFilterNames.Name,
                   gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.Name,
                   criteria.CellLineFilters.Name)
                );


                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.MgmtStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.IdhStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.IdhMutation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.GeneExpressionSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    CellLineFilterNames.MethylationSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    CellLineFilterNames.GcimpMethylation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().CellLine.MolecularData.GcimpMethylation,
                    criteria.CellLineFilters.GcimpMethylation)
                );
            }

            if (criteria.OrganoidFilters != null)
            {
                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    OrganoidFilterNames.ReferenceId,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.ReferenceId,
                    criteria.OrganoidFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    OrganoidFilterNames.Medium,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.Medium,
                    criteria.OrganoidFilters.Medium)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    OrganoidFilterNames.Tumorigenicity,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.Tumorigenicity,
                    criteria.OrganoidFilters.Tumorigenicity)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    OrganoidFilterNames.Intervention,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.Interventions.First().Type,
                    criteria.OrganoidFilters.Intervention)
                );


                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    OrganoidFilterNames.MgmtStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    OrganoidFilterNames.IdhStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    OrganoidFilterNames.IdhMutation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    OrganoidFilterNames.GeneExpressionSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    OrganoidFilterNames.MethylationSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.OrganoidFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    OrganoidFilterNames.GcimpMethylation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Organoid.MolecularData.GcimpMethylation,
                    criteria.OrganoidFilters.GcimpMethylation)
                );
            }

            if (criteria.XenograftFilters != null)
            {
                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    XenograftFilterNames.ReferenceId,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.ReferenceId,
                    criteria.XenograftFilters.ReferenceId)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    XenograftFilterNames.MouseStrain,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MouseStrain,
                    criteria.XenograftFilters.MouseStrain)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    XenograftFilterNames.Tumorigenicity,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.Tumorigenicity,
                    criteria.XenograftFilters.Tumorigenicity)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    XenograftFilterNames.TumorGrowthForm,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.TumorGrowthForm.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.TumorGrowthForm)
                );

                _filters.Add(new MultiPropertyRangeFilter<GeneIndex, double?>(
                    XenograftFilterNames.SurvivalDays,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.SurvivalDaysFrom,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.SurvivalDaysTo,
                    criteria.XenograftFilters.SurvivalDays?.From,
                    criteria.XenograftFilters.SurvivalDays?.To)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    XenograftFilterNames.Intervention,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.Interventions.First().Type,
                    criteria.XenograftFilters.Intervention)
                );


                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    XenograftFilterNames.MgmtStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.MgmtStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    XenograftFilterNames.IdhStatus,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MolecularData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.IdhStatus)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    XenograftFilterNames.IdhMutation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MolecularData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.IdhMutation)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    XenograftFilterNames.GeneExpressionSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.GeneExpressionSubtype)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    XenograftFilterNames.MethylationSubtype,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.XenograftFilters.MethylationSubtype)
                );

                _filters.Add(new BooleanFilter<GeneIndex>(
                    XenograftFilterNames.GcimpMethylation,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Xenograft.MolecularData.GcimpMethylation,
                    criteria.XenograftFilters.GcimpMethylation)
                );
            }

            if (criteria.GeneFilters != null)
            {
                _filters.Add(new EqualityFilter<GeneIndex, int>(
                    GeneFilterNames.Id,
                    gene => gene.Id,
                    criteria.GeneFilters.Id)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    GeneFilterNames.Symbol,
                    gene => gene.Symbol,
                    criteria.GeneFilters.Symbol)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    GeneFilterNames.Biotype,
                    gene => gene.Biotype.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Biotype)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    GeneFilterNames.Chromosome,
                    gene => gene.Chromosome.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Chromosome)
                );

                _filters.Add(new MultiPropertyRangeFilter<GeneIndex, int?>(
                    GeneFilterNames.Position,
                    gene => gene.Start,
                    gene => gene.End,
                    criteria.GeneFilters.Position?.From,
                    criteria.GeneFilters.Position?.To)
                );
            }

            if (criteria.MutationFilters != null)
            {
                _filters.Add(new EqualityFilter<GeneIndex, long>(
                    MutationFilterNames.Id,
                    gene => gene.Mutations.First().Id,
                    criteria.MutationFilters.Id)
                );

                _filters.Add(new SimilarityFilter<GeneIndex, string>(
                    MutationFilterNames.Code,
                    gene => gene.Mutations.First().Code,
                    criteria.MutationFilters.Code)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    MutationFilterNames.Type,
                    gene => gene.Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    MutationFilterNames.Chromosome,
                    gene => gene.Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome)
                );

                _filters.Add(new MultiPropertyRangeFilter<GeneIndex, int?>(
                    MutationFilterNames.Position,
                    gene => gene.Mutations.First().Start,
                    gene => gene.Mutations.First().End,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    MutationFilterNames.Impact,
                    gene => gene.Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact)
                );

                _filters.Add(new EqualityFilter<GeneIndex, object>(
                    MutationFilterNames.Consequence,
                    gene => gene.Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence)
                );
            }
        }
    }
}
