using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

using OrganoidIntervention = Unite.Data.Entities.Specimens.Organoids.Intervention;
using XenograftIntervention = Unite.Data.Entities.Specimens.Xenografts.Intervention;

namespace Unite.Composer.Download;

public class SpecimensTsvService
{
    private readonly DomainDbContext _dbContext;


    public SpecimensTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }
    

    public async Task<string> GetTissuesData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.Tissue.Source)
            .Where(entity => entity.Tissue != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateSpecimensMap();
        
        MapTissues(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetTissuesDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetTissuesData(specimenIds);
    }

    public async Task<string> GetTissuesDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetTissuesData(specimenIds);
    }


    public async Task<string> GetCellLinesData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.CellLine.Info)
            .Where(entity => entity.CellLine != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateSpecimensMap();

        MapCellLines(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetCellLinesDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetCellLinesData(specimenIds);
    }


    public async Task<string> GetOrganoidsData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.Organoid)
            .Where(entity => entity.Organoid != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateSpecimensMap();;

        MapOrganoids(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetOrganoidsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetOrganoidsData(specimenIds);
    }


    public async Task<string> GetXenograftsData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.Xenograft)
            .Where(entity => entity.Xenograft != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateSpecimensMap();

        MapXenografts(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetXenograftsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetXenograftsData(specimenIds);
    }


    public async Task<string> GetOrganoidInterventionsData(IEnumerable<int> ids)
    {
        var entities = await CreateOrganoidInterventionsQuery()
            .Where(entity => ids.Contains(entity.Organoid.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<OrganoidIntervention>();

        MapInterventions(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetOrganoidInterventionsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetOrganoidInterventionsData(specimenIds);
    }


    public async Task<string> GetXenograftInterventionsData(IEnumerable<int> ids)
    {
        var entities = await CreateXenograftInterventionsQuery()
            .Where(entity => ids.Contains(entity.Xenograft.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<XenograftIntervention>();

        MapInterventions(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetXenograftInterventionsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetXenograftInterventionsData(specimenIds);
    }


    public async Task<string> GetCellLineDrugScreeningsData(IEnumerable<int> ids)
    {
        var entities = await CreateDrugScreeningsQuery()
            .Where(entity => entity.Specimen.CellLine != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>();

        MapDrugScreenings(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetCellLineDrugScreeningsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetCellLineDrugScreeningsData(specimenIds);
    }


    public async Task<string> GetOrganoidDrugScreeningsData(IEnumerable<int> ids)
    {
        var entities = await CreateDrugScreeningsQuery()
            .Where(entity => entity.Specimen.Organoid != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>();

        MapDrugScreenings(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetOrganoidDrugScreeningsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetOrganoidDrugScreeningsData(specimenIds);
    }


    public async Task<string> GetXenograftDrugScreeningsData(IEnumerable<int> ids)
    {
        var entities = await CreateDrugScreeningsQuery()
            .Where(entity => entity.Specimen.Xenograft != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>();

        MapDrugScreenings(ref map);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetXenograftDrugScreeningsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetXenograftDrugScreeningsData(specimenIds);
    }


    private async Task<int[]> GetSpecimenIdsForDonors(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetSpecimenIdsForImages(IEnumerable<int> ids)
    { 
        var donorIds = await _dbContext.Set<Image>().AsNoTracking()
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();

        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Tissue)
            .Where(entity => entity.Tissue != null && entity.Tissue.TypeId != TissueType.Control)
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }


    private IQueryable<Specimen> CreateSpecimensQuery()
    {
        return _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MolecularData)
            .Include(entity => entity.Parent.Tissue)
            .Include(entity => entity.Parent.CellLine)
            .Include(entity => entity.Parent.Organoid)
            .Include(entity => entity.Parent.Xenograft);
    }

    private IQueryable<OrganoidIntervention> CreateOrganoidInterventionsQuery()
    {
        return _dbContext.Set<OrganoidIntervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Organoid.Specimen.Donor)
            .Include(entity => entity.Organoid.Specimen.Organoid);
    }

    private IQueryable<XenograftIntervention> CreateXenograftInterventionsQuery()
    {
        return _dbContext.Set<XenograftIntervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Xenograft.Specimen.Donor)
            .Include(entity => entity.Xenograft.Specimen.Xenograft);
    }

    private IQueryable<DrugScreening> CreateDrugScreeningsQuery()
    {
        return _dbContext.Set<DrugScreening>().AsNoTracking()
            .Include(entity => entity.Drug)
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.Specimen.Tissue)
            .Include(entity => entity.Specimen.CellLine)
            .Include(entity => entity.Specimen.Organoid)
            .Include(entity => entity.Specimen.Xenograft);
    }


    private static ClassMap<Specimen> CreateSpecimensMap()
    {
        return new ClassMap<Specimen>()
            .Map(entity => entity.ReferenceId, "specimen_id")
            .Map(entity => entity.Type, "specimen_type")
            .Map(entity => entity.Parent.ReferenceId, "parent_id")
            .Map(entity => entity.Parent.Type, "parent_type")
            .Map(entity => entity.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.CreationDate, "creation_date")
            .Map(entity => entity.CreationDay, "creation_day");
    }

    private static void MapTissues(ref ClassMap<Specimen> map)
    {
        map.Map(entity => entity.Tissue.TypeId, "type")
           .Map(entity => entity.Tissue.TumorTypeId, "tumor_type")
           .Map(entity => entity.Tissue.Source.Value, "source");

        MapMolecularData(ref map);
    }

    private static void MapCellLines(ref ClassMap<Specimen> map)
    {
        map.Map(entity => entity.CellLine.SpeciesId, "species")
           .Map(entity => entity.CellLine.TypeId, "type")
           .Map(entity => entity.CellLine.CultureTypeId, "culture_type");
        
        MapMolecularData(ref map);

        map.Map(entity => entity.CellLine.Info.Name, "public_name")
           .Map(entity => entity.CellLine.Info.DepositorName, "depositor_name")
           .Map(entity => entity.CellLine.Info.DepositorEstablishment, "depositor_establishment")
           .Map(entity => entity.CellLine.Info.EstablishmentDate, "establishment_date")
           .Map(entity => entity.CellLine.Info.PubMedLink, "pubmed_link")
           .Map(entity => entity.CellLine.Info.AtccLink, "atcc_link")
           .Map(entity => entity.CellLine.Info.ExPasyLink, "expasy_link");
    }

    private static void MapOrganoids(ref ClassMap<Specimen> map)
    {
        map.Map(entity => entity.Organoid.ImplantedCellsNumber, "implanted_cells_number")
           .Map(entity => entity.Organoid.Tumorigenicity, "tumorigenicity")
           .Map(entity => entity.Organoid.Medium, "medium");

        MapMolecularData(ref map);
    }

    private static void MapXenografts(ref ClassMap<Specimen> map)
    {
        map.Map(entity => entity.Xenograft.MouseStrain, "mouse_strain")
           .Map(entity => entity.Xenograft.GroupSize, "group_size")
           .Map(entity => entity.Xenograft.ImplantTypeId, "implant_type")
           .Map(entity => entity.Xenograft.TissueLocationId, "tissue_location")
           .Map(entity => entity.Xenograft.ImplantedCellsNumber, "implanted_cells_number")
           .Map(entity => entity.Xenograft.Tumorigenicity, "tumorigenicity")
           .Map(entity => entity.Xenograft.TumorGrowthFormId, "tumor_growth_form")
           .Map(entity => entity.Xenograft.SurvivalDaysFrom, "survival_days_from")
           .Map(entity => entity.Xenograft.SurvivalDaysTo, "survival_days_to");

        MapMolecularData(ref map);
    }

    private static void MapMolecularData(ref ClassMap<Specimen> map)
    {
        map.Map(entity => entity.MolecularData.MgmtStatusId, "mgmt")
           .Map(entity => entity.MolecularData.IdhStatusId, "idh")
           .Map(entity => entity.MolecularData.IdhMutationId, "idh_mutation")
           .Map(entity => entity.MolecularData.MethylationSubtypeId, "methylation_subtype")
           .Map(entity => entity.MolecularData.GcimpMethylation, "g-cimp_methylation");
    }

    private static void MapInterventions(ref ClassMap<OrganoidIntervention> map)
    {
        map.Map(entity => entity.Organoid.Specimen.Donor.ReferenceId, "donor_id")
           .Map(entity => entity.Organoid.Specimen.ReferenceId, "specimen_id")
           .Map(entity => entity.Organoid.Specimen.Type, "specimen_type")
           .Map(entity => entity.Type.Name, "type")
           .Map(entity => entity.Type.Description, "description")
           .Map(entity => entity.StartDate, "start_date")
           .Map(entity => entity.StartDay, "start_day")
           .Map(entity => entity.EndDate, "end_date")
           .Map(entity => entity.DurationDays, "duration_days")
           .Map(entity => entity.Results, "results");
    }

    private static void MapInterventions(ref ClassMap<XenograftIntervention> map)
    {
        map.Map(entity => entity.Xenograft.Specimen.Donor.ReferenceId, "donor_id")
           .Map(entity => entity.Xenograft.Specimen.ReferenceId, "specimen_id")
           .Map(entity => entity.Xenograft.Specimen.Type, "specimen_type")
           .Map(entity => entity.Type.Name, "type")
           .Map(entity => entity.Type.Description, "description")
           .Map(entity => entity.StartDate, "start_date")
           .Map(entity => entity.StartDay, "start_day")
           .Map(entity => entity.EndDate, "end_date")
           .Map(entity => entity.DurationDays, "duration_days")
           .Map(entity => entity.Results, "results");
    }

    private static void MapDrugScreenings(ref ClassMap<DrugScreening> map)
    {
        map.Map(entity => entity.Specimen.Donor.ReferenceId, "donor_id")
           .Map(entity => entity.Specimen.ReferenceId, "specimen_id")
           .Map(entity => entity.Specimen.Type, "specimen_type")
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
}
