using System.Linq.Expressions;
using Unite.Data.Entities.Specimens;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

using OrganoidIntervention = Unite.Data.Entities.Specimens.Organoids.Intervention;
using XenograftIntervention = Unite.Data.Entities.Specimens.Xenografts.Intervention;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class SpecimenMappingExtensions
{
    public static ClassMap<Specimen> MapTissues(this ClassMap<Specimen> map)
    {
        return map
            .MapSpecimen()
            .Map(entity => entity.Tissue.TypeId, "type")
            .Map(entity => entity.Tissue.TumorTypeId, "tumor_type")
            .Map(entity => entity.Tissue.Source.Value, "source")
            .MapMolecularData();
    }

    public static ClassMap<Specimen> MapCellLines(this ClassMap<Specimen> map)
    {
        return map
            .MapSpecimen()
            .Map(entity => entity.CellLine.SpeciesId, "species")
            .Map(entity => entity.CellLine.TypeId, "type")
            .Map(entity => entity.CellLine.CultureTypeId, "culture_type")
            .MapMolecularData()
            .Map(entity => entity.CellLine.Info.Name, "public_name")
            .Map(entity => entity.CellLine.Info.DepositorName, "depositor_name")
            .Map(entity => entity.CellLine.Info.DepositorEstablishment, "depositor_establishment")
            .Map(entity => entity.CellLine.Info.EstablishmentDate, "establishment_date")
            .Map(entity => entity.CellLine.Info.PubMedLink, "pubmed_link")
            .Map(entity => entity.CellLine.Info.AtccLink, "atcc_link")
            .Map(entity => entity.CellLine.Info.ExPasyLink, "expasy_link");
    }

    public static ClassMap<Specimen> MapOrganoids(this ClassMap<Specimen> map)
    {
        return map
            .MapSpecimen()
            .Map(entity => entity.Organoid.ImplantedCellsNumber, "implanted_cells_number")
            .Map(entity => entity.Organoid.Tumorigenicity, "tumorigenicity")
            .Map(entity => entity.Organoid.Medium, "medium")
            .MapMolecularData();
    }

    public static ClassMap<Specimen> MapXenografts(this ClassMap<Specimen> map)
    {
        return map
            .MapSpecimen()
            .Map(entity => entity.Xenograft.MouseStrain, "mouse_strain")
            .Map(entity => entity.Xenograft.GroupSize, "group_size")
            .Map(entity => entity.Xenograft.ImplantTypeId, "implant_type")
            .Map(entity => entity.Xenograft.TissueLocationId, "tissue_location")
            .Map(entity => entity.Xenograft.ImplantedCellsNumber, "implanted_cells_number")
            .Map(entity => entity.Xenograft.Tumorigenicity, "tumorigenicity")
            .Map(entity => entity.Xenograft.TumorGrowthFormId, "tumor_growth_form")
            .Map(entity => entity.Xenograft.SurvivalDaysFrom, "survival_days_from")
            .Map(entity => entity.Xenograft.SurvivalDaysTo, "survival_days_to")
            .MapMolecularData();
    }

    public static ClassMap<OrganoidIntervention> MapInterventions(this ClassMap<OrganoidIntervention> map)
    {
        return map
            .MapSpecimen(entity => entity.Organoid.Specimen)
            .Map(entity => entity.Type.Name, "type")
            .Map(entity => entity.Type.Description, "description")
            .Map(entity => entity.StartDate, "start_date")
            .Map(entity => entity.StartDay, "start_day")
            .Map(entity => entity.EndDate, "end_date")
            .Map(entity => entity.DurationDays, "duration_days")
            .Map(entity => entity.Results, "results");
    }

    public static ClassMap<XenograftIntervention> MapInterventions(this ClassMap<XenograftIntervention> map)
    {
        return map
            .MapSpecimen(entity => entity.Xenograft.Specimen)
            .Map(entity => entity.Type.Name, "type")
            .Map(entity => entity.Type.Description, "description")
            .Map(entity => entity.StartDate, "start_date")
            .Map(entity => entity.StartDay, "start_day")
            .Map(entity => entity.EndDate, "end_date")
            .Map(entity => entity.DurationDays, "duration_days")
            .Map(entity => entity.Results, "results");
    }

    public static ClassMap<DrugScreening> MapDrugScreenings(this ClassMap<DrugScreening> map)
    {
        return map
            .MapSpecimen(entity => entity.Specimen)
            .Map(entity => entity.Drug.Name, "drug_name")
            .Map(entity => entity.Drug.Description, "drug_description")
            .Map(entity => entity.Dss, "dss")
            .Map(entity => entity.DssSelective, "dss_selective")
            .Map(entity => entity.MinConcentration, "min_concentration")
            .Map(entity => entity.MaxConcentration, "max_concentration")
            .Map(entity => entity.AbsIC25, "abs_ic25")
            .Map(entity => entity.AbsIC50, "abs_ic50")
            .Map(entity => entity.AbsIC75, "abs_ic75");
    }


    private static ClassMap<Specimen> MapSpecimen(this ClassMap<Specimen> map)
    {
        return map
            .Map(entity => entity.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.ReferenceId, "specimen_id")
            .Map(entity => entity.TypeId, "specimen_type")
            .Map(entity => entity.Parent.ReferenceId, "parent_id")
            .Map(entity => entity.Parent.TypeId, "parent_type")
            .Map(entity => entity.CreationDate, "creation_date")
            .Map(entity => entity.CreationDay, "creation_day");
    }

    private static ClassMap<T> MapSpecimen<T>(this ClassMap<T> map, Expression<Func<T, Specimen>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.Donor.ReferenceId), "donor_id")
            .Map(path.Join(entity => entity.ReferenceId), "specimen_id")
            .Map(path.Join(entity => entity.TypeId), "specimen_type");
    }

    private static ClassMap<Specimen> MapMolecularData(this ClassMap<Specimen> map)
    {
        return map
            .Map(entity => entity.MolecularData.MgmtStatusId, "mgmt")
            .Map(entity => entity.MolecularData.IdhStatusId, "idh")
            .Map(entity => entity.MolecularData.IdhMutationId, "idh_mutation")
            .Map(entity => entity.MolecularData.MethylationSubtypeId, "methylation_subtype")
            .Map(entity => entity.MolecularData.GcimpMethylation, "g-cimp_methylation");
    }
}
