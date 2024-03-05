using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Enums;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class SpecimensTsvService : TsvServiceBase
{
    public SpecimensTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<string> GetData(IEnumerable<int> ids, SpecimenType typeId)
    {
        if (typeId == SpecimenType.Material)
            return await GetMaterialsData(ids);
        else if (typeId == SpecimenType.Line)
            return await GetLinesData(ids);
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
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateInterventionsQuery(dbContext)
            .Where(entity => entity.Specimen.TypeId == typeId)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Intervention>().MapInterventions();

        return TsvWriter.Write(entities, map);
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
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateDrugScreeningsQuery(dbContext)
            .Where(entity => entity.Specimen.TypeId == typeId)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
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


    private async Task<string> GetMaterialsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        
        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.Material.Source)
            .Where(entity => entity.TypeId != SpecimenType.Material)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapMaterials();
        
        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetLinesData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.Line.Info)
            .Where(entity => entity.TypeId == SpecimenType.Line)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapLines();

        return TsvWriter.Write(entities, map);
    }

    private async Task<string> GetOrganoidsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery(dbContext)
            .Include(entity => entity.Organoid)
            .Where(entity => entity.TypeId == SpecimenType.Organoid)
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
            .Where(entity => entity.TypeId == SpecimenType.Xenograft)
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
            .Include(entity => entity.Parent.Material)
            .Include(entity => entity.Parent.Line)
            .Include(entity => entity.Parent.Organoid)
            .Include(entity => entity.Parent.Xenograft);
    }

    private static IQueryable<Intervention> CreateInterventionsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Intervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.Specimen.Line)
            .Include(entity => entity.Specimen.Organoid)
            .Include(entity => entity.Specimen.Xenograft);
    }

    private static IQueryable<DrugScreening> CreateDrugScreeningsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<DrugScreening>().AsNoTracking()
            .Include(entity => entity.Drug)
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.Specimen.Line)
            .Include(entity => entity.Specimen.Organoid)
            .Include(entity => entity.Specimen.Xenograft);
    }
}
