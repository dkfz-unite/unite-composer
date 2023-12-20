using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Enums;
using Unite.Essentials.Tsv;

using OrganoidIntervention = Unite.Data.Entities.Specimens.Organoids.Intervention;
using XenograftIntervention = Unite.Data.Entities.Specimens.Xenografts.Intervention;

namespace Unite.Composer.Download.Tsv.Mapping;

public class SpecimensTsvService : TsvServiceBase
{
    public SpecimensTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<string> GetData(IEnumerable<int> ids, SpecimenType typeId)
    {
        if (typeId == SpecimenType.Tissue)
            return await GetTissuesData(ids);
        else if (typeId == SpecimenType.CellLine)
            return await GetCellLinesData(ids);
        else if (typeId == SpecimenType.Organoid)
            return await GetOrganoidsData(ids);
        else if (typeId == SpecimenType.Xenograft)
            return await GetXenograftsData(ids);

        return null;
    }

    public async Task<string> GetDataForDonors(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids, typeId);

        return await GetData(specimenIds, typeId);
    }

    public async Task<string> GetDataForImages(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _imagesRepository.GetRelatedSpecimens(ids);

        return await GetData(specimenIds, typeId);
    }

    public async Task<string> GetDataForGenes(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _genesRepository.GetRelatedSpecimens(ids);

        return await GetData(specimenIds, typeId);
    }

    public async Task<string> GetDataForVariants<TV>(IEnumerable<long> ids, SpecimenType typeId)
        where TV : Variant
    {
        var specimenIds = await _variantsRepository.GetRelatedSpecimens<TV>(ids);

        return await GetData(specimenIds, typeId);
    }
    

    public async Task<string> GetInterventionsData(IEnumerable<int> ids, SpecimenType typeId)
    {
        if (typeId == SpecimenType.Organoid)
            return await GetOrganoidInterventionsData(ids);
        else if (typeId == SpecimenType.Xenograft)
            return await GetXenograftInterventionsData(ids);

        return null;
    }

    public async Task<string> GetInterventionsDataForDonors(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids, typeId);

        return await GetInterventionsData(specimenIds, typeId);
    }

    public async Task<string> GetInterventionsDataForGenes(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _genesRepository.GetRelatedSpecimens(ids);

        return await GetInterventionsData(specimenIds, typeId);
    }

    public async Task<string> GetInterventionsDataForVariants<TV>(IEnumerable<long> ids, SpecimenType typeId)
        where TV : Variant
    {
        var specimenIds = await _variantsRepository.GetRelatedSpecimens<TV>(ids);

        return await GetInterventionsData(specimenIds, typeId);
    }


    public async Task<string> GetDrugsScreeningsData(IEnumerable<int> ids, SpecimenType typeId)
    {
        if (typeId == SpecimenType.CellLine)
            return await GetCellLineDrugScreeningsData(ids);
        else if (typeId == SpecimenType.Organoid)
            return await GetOrganoidDrugScreeningsData(ids);
        else if (typeId == SpecimenType.Xenograft)
            return await GetXenograftDrugScreeningsData(ids);

        return null;
    }

    public async Task<string> GetDrugsScreeningsDataForDonors(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids, typeId);

        return await GetDrugsScreeningsData(specimenIds, typeId);
    }

    public async Task<string> GetDrugsScreeningsDataForGenes(IEnumerable<int> ids, SpecimenType typeId)
    {
        var specimenIds = await _genesRepository.GetRelatedSpecimens(ids);

        return await GetDrugsScreeningsData(specimenIds, typeId);
    }

    public async Task<string> GetDrugsScreeningsDataForVariants<TV>(IEnumerable<long> ids, SpecimenType typeId)
        where TV : Variant
    {
        var specimenIds = await _variantsRepository.GetRelatedSpecimens<TV>(ids);

        return await GetDrugsScreeningsData(specimenIds, typeId);
    }


    private async Task<string> GetTissuesData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        
        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.Tissue.Source)
            .Where(entity => entity.Tissue != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapTissues();
        
        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetCellLinesData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.CellLine.Info)
            .Where(entity => entity.CellLine != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapCellLines();

        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetOrganoidsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.Organoid)
            .Where(entity => entity.Organoid != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapOrganoids();

        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetXenograftsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.Xenograft)
            .Where(entity => entity.Xenograft != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapXenografts();

        return TsvWriter.Write(entities, map);
    }

    private static IQueryable<Specimen> CreateQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MolecularData)
            .Include(entity => entity.Parent.Tissue)
            .Include(entity => entity.Parent.CellLine)
            .Include(entity => entity.Parent.Organoid)
            .Include(entity => entity.Parent.Xenograft);
    }


    private async Task<string> GetOrganoidInterventionsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateOrganoidInterventionsQuery(dbContext)
            .Where(entity => ids.Contains(entity.Organoid.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<OrganoidIntervention>().MapInterventions();

        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetXenograftInterventionsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateXenograftInterventionsQuery(dbContext)
            .Where(entity => ids.Contains(entity.Xenograft.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<XenograftIntervention>().MapInterventions();

        return TsvWriter.Write(entities, map);
    }

    private static IQueryable<OrganoidIntervention> CreateOrganoidInterventionsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<OrganoidIntervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Organoid.Specimen.Donor)
            .Include(entity => entity.Organoid.Specimen.Organoid);
    }

    private static IQueryable<XenograftIntervention> CreateXenograftInterventionsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<XenograftIntervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Xenograft.Specimen.Donor)
            .Include(entity => entity.Xenograft.Specimen.Xenograft);
    }


    private async Task<string> GetCellLineDrugScreeningsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateDrugScreeningsQuery(dbContext)
            .Where(entity => entity.Specimen.CellLine != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetOrganoidDrugScreeningsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateDrugScreeningsQuery(dbContext)
            .Where(entity => entity.Specimen.Organoid != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetXenograftDrugScreeningsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateDrugScreeningsQuery(dbContext)
            .Where(entity => entity.Specimen.Xenograft != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
    }

    private static IQueryable<DrugScreening> CreateDrugScreeningsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<DrugScreening>().AsNoTracking()
            .Include(entity => entity.Drug)
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.Specimen.Tissue)
            .Include(entity => entity.Specimen.CellLine)
            .Include(entity => entity.Specimen.Organoid)
            .Include(entity => entity.Specimen.Xenograft);
    }
}
