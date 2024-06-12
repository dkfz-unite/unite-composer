using System.Linq.Expressions;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Analysis.Drugs;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class SpecimenMappingExtensions
{
    public static ClassMap<Specimen> MapMaterials(this ClassMap<Specimen> map)
    {
        return map
            .MapSpecimen()
            .Map(entity => entity.Material.TypeId, "type")
            .Map(entity => entity.Material.TumorTypeId, "tumor_type")
            .Map(entity => entity.Material.Source.Value, "source")
            .MapMolecularData();
    }

    public static ClassMap<Specimen> MapLines(this ClassMap<Specimen> map)
    {
        return map
            .MapSpecimen()
            .Map(entity => entity.Line.CellsSpeciesId, "cells_species")
            .Map(entity => entity.Line.CellsTypeId, "cells_type")
            .Map(entity => entity.Line.CellsCultureTypeId, "cells_culture_type")
            .MapMolecularData()
            .Map(entity => entity.Line.Info.Name, "public_name")
            .Map(entity => entity.Line.Info.DepositorName, "depositor_name")
            .Map(entity => entity.Line.Info.DepositorEstablishment, "depositor_establishment")
            .Map(entity => entity.Line.Info.EstablishmentDate, "establishment_date")
            .Map(entity => entity.Line.Info.PubMedLink, "pubmed_link")
            .Map(entity => entity.Line.Info.AtccLink, "atcc_link")
            .Map(entity => entity.Line.Info.ExPasyLink, "expasy_link");
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
            .Map(entity => entity.Xenograft.ImplantLocationId, "implant_location")
            .Map(entity => entity.Xenograft.ImplantedCellsNumber, "implanted_cells_number")
            .Map(entity => entity.Xenograft.Tumorigenicity, "tumorigenicity")
            .Map(entity => entity.Xenograft.TumorGrowthFormId, "tumor_growth_form")
            .Map(entity => entity.Xenograft.SurvivalDaysFrom, "survival_days_from")
            .Map(entity => entity.Xenograft.SurvivalDaysTo, "survival_days_to")
            .MapMolecularData();
    }

    public static ClassMap<Intervention> MapInterventions(this ClassMap<Intervention> map)
    {
        return map
            .MapSpecimen(entity => entity.Specimen)
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
            .MapSpecimen(entity => entity.Sample.Specimen)
            .Map(entity => entity.Entity.Name, "drug_name")
            .Map(entity => entity.Entity.Description, "drug_description")
            .Map(entity => entity.Gof, "gof")
            .Map(entity => entity.Dss, "dss")
            .Map(entity => entity.DssS, "dsss")
            .Map(entity => entity.MinDose, "min_dose")
            .Map(entity => entity.MaxDose, "max_dose")
            .Map(entity => entity.Dose25, "dose_25")
            .Map(entity => entity.Dose50, "dose_50")
            .Map(entity => entity.Dose75, "dose_75");
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
