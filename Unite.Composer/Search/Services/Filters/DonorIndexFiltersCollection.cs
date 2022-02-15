using System.Linq;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Search.Services.Filters
{
    public class DonorIndexFiltersCollection : FiltersCollection<DonorIndex>
    {
        public DonorIndexFiltersCollection(SearchCriteria criteria) : base()
        {
            var donorFilters = new DonorFilters<DonorIndex>(criteria.DonorFilters, donor => donor);
            var mriImageFilters = new MriImageFilters<DonorIndex>(criteria.MriImageFilters, donor => donor.Images.First());
            var tissueFilters = new TissueFilters<DonorIndex>(criteria.TissueFilters, donor => donor.Specimens.First());
            var cellLineFilters = new CellLineFilters<DonorIndex>(criteria.CellLineFilters, donor => donor.Specimens.First());
            var organoidFilters = new OrganoidFilters<DonorIndex>(criteria.OrganoidFilters, donor => donor.Specimens.First());
            var xenograftFilters = new XenograftFilters<DonorIndex>(criteria.XenograftFilters, donor => donor.Specimens.First());
            var geneFilters = new GeneFilters<DonorIndex>(criteria.GeneFilters, donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene);
            var mutationFilters = new MutationFilters<DonorIndex>(criteria.MutationFilters, donor => donor.Specimens.First().Mutations.First());

            _filters.AddRange(donorFilters.All());
            _filters.AddRange(mriImageFilters.All());
            _filters.AddRange(tissueFilters.All());
            _filters.AddRange(cellLineFilters.All());
            _filters.AddRange(organoidFilters.All());
            _filters.AddRange(xenograftFilters.All());
            _filters.AddRange(geneFilters.All());
            _filters.AddRange(mutationFilters.All());

            //if (criteria.DonorFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, int>(
            //        DonorFilterNames.Id,
            //        donor => donor.Id,
            //        criteria.DonorFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        DonorFilterNames.ReferenceId,
            //        donor => donor.ReferenceId,
            //        criteria.DonorFilters.ReferenceId)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        DonorFilterNames.Diagnosis,
            //        donor => donor.ClinicalData.Diagnosis,
            //        criteria.DonorFilters.Diagnosis)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        DonorFilterNames.Gender,
            //        donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix),
            //        criteria.DonorFilters.Gender)
            //    );

            //    _filters.Add(new RangeFilter<DonorIndex, int?>(
            //        DonorFilterNames.Age,
            //        donor => donor.ClinicalData.Age,
            //        criteria.DonorFilters.Age?.From,
            //        criteria.DonorFilters.Age?.To)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        DonorFilterNames.VitalStatus,
            //        donor => donor.ClinicalData.VitalStatus,
            //        criteria.DonorFilters.VitalStatus)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, object>(
            //       DonorFilterNames.Therapy,
            //       donor => donor.Treatments.First().Therapy,
            //       criteria.DonorFilters.Therapy)
            //   );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        DonorFilterNames.MtaProtected,
            //        donor => donor.MtaProtected,
            //        criteria.DonorFilters.MtaProtected)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, object>(
            //       DonorFilterNames.WorkPackage,
            //       donor => donor.WorkPackages.First().Name.Suffix(_keywordSuffix),
            //       criteria.DonorFilters.WorkPackage)
            //   );
            //}

            if (criteria.SpecimenFilters != null)
            {
                _filters.Add(new EqualityFilter<DonorIndex, int>(
                    SpecimenFilterNames.Id,
                    donor => donor.Specimens.First().Id,
                    criteria.SpecimenFilters.Id)
                );
            }

            //if (criteria.TissueFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, int>(
            //        SpecimenFilterNames.Id,
            //        donor => donor.Specimens.First().Id,
            //        criteria.TissueFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        TissueFilterNames.ReferenceId,
            //        donor => donor.Specimens.First().Tissue.ReferenceId,
            //        criteria.TissueFilters.ReferenceId)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.Type,
            //        donor => donor.Specimens.First().Tissue.Type.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.Type)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.TumorType,
            //        donor => donor.Specimens.First().Tissue.TumorType.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.TumorType)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        TissueFilterNames.Source,
            //        donor => donor.Specimens.First().Tissue.Source,
            //        criteria.TissueFilters.Source)
            //    );


            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.MgmtStatus,
            //        donor => donor.Specimens.First().Tissue.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.MgmtStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.IdhStatus,
            //        donor => donor.Specimens.First().Tissue.MolecularData.IdhStatus.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.IdhStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.IdhMutation,
            //        donor => donor.Specimens.First().Tissue.MolecularData.IdhMutation.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.IdhMutation)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.GeneExpressionSubtype,
            //        donor => donor.Specimens.First().Tissue.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.GeneExpressionSubtype)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        TissueFilterNames.MethylationSubtype,
            //        donor => donor.Specimens.First().Tissue.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.MethylationSubtype)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        TissueFilterNames.GcimpMethylation,
            //        donor => donor.Specimens.First().Tissue.MolecularData.GcimpMethylation,
            //        criteria.TissueFilters.GcimpMethylation)
            //    );
            //}

            //if (criteria.CellLineFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, int>(
            //        SpecimenFilterNames.Id,
            //        donor => donor.Specimens.First().Id,
            //        criteria.CellLineFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        CellLineFilterNames.ReferenceId,
            //        donor => donor.Specimens.First().CellLine.ReferenceId,
            //        criteria.CellLineFilters.ReferenceId)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.Species,
            //        donor => donor.Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.Species)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.Type,
            //        donor => donor.Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.Type)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.CultureType,
            //        donor => donor.Specimens.First().CellLine.CultureType.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.CultureType)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //       CellLineFilterNames.Name,
            //       donor => donor.Specimens.First().CellLine.Name,
            //       criteria.CellLineFilters.Name)
            //    );


            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.MgmtStatus,
            //        donor => donor.Specimens.First().CellLine.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.MgmtStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.IdhStatus,
            //        donor => donor.Specimens.First().CellLine.MolecularData.IdhStatus.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.IdhStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.IdhMutation,
            //        donor => donor.Specimens.First().CellLine.MolecularData.IdhMutation.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.IdhMutation)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.GeneExpressionSubtype,
            //        donor => donor.Specimens.First().CellLine.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.GeneExpressionSubtype)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        CellLineFilterNames.MethylationSubtype,
            //        donor => donor.Specimens.First().CellLine.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.MethylationSubtype)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        CellLineFilterNames.GcimpMethylation,
            //        donor => donor.Specimens.First().CellLine.MolecularData.GcimpMethylation,
            //        criteria.CellLineFilters.GcimpMethylation)
            //    );
            //}

            //if (criteria.OrganoidFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, int>(
            //        SpecimenFilterNames.Id,
            //        donor => donor.Specimens.First().Id,
            //        criteria.OrganoidFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        OrganoidFilterNames.ReferenceId,
            //        donor => donor.Specimens.First().Organoid.ReferenceId,
            //        criteria.OrganoidFilters.ReferenceId)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        OrganoidFilterNames.Medium,
            //        donor => donor.Specimens.First().Organoid.Medium,
            //        criteria.OrganoidFilters.Medium)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        OrganoidFilterNames.Tumorigenicity,
            //        donor => donor.Specimens.First().Organoid.Tumorigenicity,
            //        criteria.OrganoidFilters.Tumorigenicity)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        OrganoidFilterNames.Intervention,
            //        donor => donor.Specimens.First().Organoid.Interventions.First().Type,
            //        criteria.OrganoidFilters.Intervention)
            //    );


            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        OrganoidFilterNames.MgmtStatus,
            //        donor => donor.Specimens.First().Organoid.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
            //        criteria.OrganoidFilters.MgmtStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        OrganoidFilterNames.IdhStatus,
            //        donor => donor.Specimens.First().Organoid.MolecularData.IdhStatus.Suffix(_keywordSuffix),
            //        criteria.OrganoidFilters.IdhStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        OrganoidFilterNames.IdhMutation,
            //        donor => donor.Specimens.First().Organoid.MolecularData.IdhMutation.Suffix(_keywordSuffix),
            //        criteria.OrganoidFilters.IdhMutation)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        OrganoidFilterNames.GeneExpressionSubtype,
            //        donor => donor.Specimens.First().Organoid.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
            //        criteria.OrganoidFilters.GeneExpressionSubtype)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        OrganoidFilterNames.MethylationSubtype,
            //        donor => donor.Specimens.First().Organoid.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
            //        criteria.OrganoidFilters.MethylationSubtype)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        OrganoidFilterNames.GcimpMethylation,
            //        donor => donor.Specimens.First().Organoid.MolecularData.GcimpMethylation,
            //        criteria.OrganoidFilters.GcimpMethylation)
            //    );
            //}

            //if (criteria.XenograftFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, int>(
            //        SpecimenFilterNames.Id,
            //        donor => donor.Specimens.First().Id,
            //        criteria.XenograftFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        XenograftFilterNames.ReferenceId,
            //        donor => donor.Specimens.First().Xenograft.ReferenceId,
            //        criteria.XenograftFilters.ReferenceId)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        XenograftFilterNames.MouseStrain,
            //        donor => donor.Specimens.First().Xenograft.MouseStrain,
            //        criteria.XenograftFilters.MouseStrain)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        XenograftFilterNames.Tumorigenicity,
            //        donor => donor.Specimens.First().Xenograft.Tumorigenicity,
            //        criteria.XenograftFilters.Tumorigenicity)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        XenograftFilterNames.TumorGrowthForm,
            //        donor => donor.Specimens.First().Xenograft.TumorGrowthForm.Suffix(_keywordSuffix),
            //        criteria.XenograftFilters.TumorGrowthForm)
            //    );

            //    _filters.Add(new MultiPropertyRangeFilter<DonorIndex, double?>(
            //        XenograftFilterNames.SurvivalDays,
            //        donor => donor.Specimens.First().Xenograft.SurvivalDaysFrom,
            //        donor => donor.Specimens.First().Xenograft.SurvivalDaysTo,
            //        criteria.XenograftFilters.SurvivalDays?.From,
            //        criteria.XenograftFilters.SurvivalDays?.To)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        XenograftFilterNames.Intervention,
            //        donor => donor.Specimens.First().Xenograft.Interventions.First().Type,
            //        criteria.XenograftFilters.Intervention)
            //    );


            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        XenograftFilterNames.MgmtStatus,
            //        donor => donor.Specimens.First().Xenograft.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
            //        criteria.XenograftFilters.MgmtStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        XenograftFilterNames.IdhStatus,
            //        donor => donor.Specimens.First().Xenograft.MolecularData.IdhStatus.Suffix(_keywordSuffix),
            //        criteria.XenograftFilters.IdhStatus)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        XenograftFilterNames.IdhMutation,
            //        donor => donor.Specimens.First().Xenograft.MolecularData.IdhMutation.Suffix(_keywordSuffix),
            //        criteria.XenograftFilters.IdhMutation)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        XenograftFilterNames.GeneExpressionSubtype,
            //        donor => donor.Specimens.First().Xenograft.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
            //        criteria.XenograftFilters.GeneExpressionSubtype)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        XenograftFilterNames.MethylationSubtype,
            //        donor => donor.Specimens.First().Xenograft.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
            //        criteria.XenograftFilters.MethylationSubtype)
            //    );

            //    _filters.Add(new BooleanFilter<DonorIndex>(
            //        XenograftFilterNames.GcimpMethylation,
            //        donor => donor.Specimens.First().Xenograft.MolecularData.GcimpMethylation,
            //        criteria.XenograftFilters.GcimpMethylation)
            //    );
            //}

            //if (criteria.GeneFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, int>(
            //        GeneFilterNames.Id,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene.Id,
            //        criteria.GeneFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        GeneFilterNames.Symbol,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene.Symbol,
            //        criteria.GeneFilters.Symbol)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        GeneFilterNames.Biotype,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene.Biotype.Suffix(_keywordSuffix),
            //        criteria.GeneFilters.Biotype)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        GeneFilterNames.Chromosome,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene.Chromosome.Suffix(_keywordSuffix),
            //        criteria.GeneFilters.Chromosome)
            //    );

            //    _filters.Add(new MultiPropertyRangeFilter<DonorIndex, int?>(
            //        GeneFilterNames.Position,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene.Start,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Transcript.Gene.End,
            //        criteria.GeneFilters.Position?.From,
            //        criteria.GeneFilters.Position?.To)
            //    );
            //}

            //if (criteria.MutationFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<DonorIndex, long>(
            //        MutationFilterNames.Id,
            //        donor => donor.Specimens.First().Mutations.First().Id,
            //        criteria.MutationFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<DonorIndex, string>(
            //        MutationFilterNames.Code,
            //        donor => donor.Specimens.First().Mutations.First().Code,
            //        criteria.MutationFilters.Code)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        MutationFilterNames.Type,
            //        donor => donor.Specimens.First().Mutations.First().Type.Suffix(_keywordSuffix),
            //        criteria.MutationFilters.MutationType)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        MutationFilterNames.Chromosome,
            //        donor => donor.Specimens.First().Mutations.First().Chromosome.Suffix(_keywordSuffix),
            //        criteria.MutationFilters.Chromosome)
            //    );

            //    _filters.Add(new MultiPropertyRangeFilter<DonorIndex, int?>(
            //        MutationFilterNames.Position,
            //        donor => donor.Specimens.First().Mutations.First().Start,
            //        donor => donor.Specimens.First().Mutations.First().End,
            //        criteria.MutationFilters.Position?.From,
            //        criteria.MutationFilters.Position?.To)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        MutationFilterNames.Impact,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
            //        criteria.MutationFilters.Impact)
            //    );

            //    _filters.Add(new EqualityFilter<DonorIndex, object>(
            //        MutationFilterNames.Consequence,
            //        donor => donor.Specimens.First().Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
            //        criteria.MutationFilters.Consequence)
            //    );
            //}
        }
    }
}
