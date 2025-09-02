using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Enums;

namespace Unite.Composer.Download.Repositories;

public class SpecimensDataRepository : DataRepository
{
    public SpecimensDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<Specimen[]> GetSpecimens(IEnumerable<int> ids, SpecimenType type)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateSpecimensQuery(dbContext, type)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();
    }

    public async Task<Specimen[]> GetSpecimensForDonors(IEnumerable<int> ids, SpecimenType type)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids, type);

        return await GetSpecimens(specimenIds, type);
    }

    public async Task<Specimen[]> GetSpecimensForImages(IEnumerable<int> ids)
    {
        var specimenIds = await _imagesRepository.GetRelatedSpecimens(ids);

        return await GetSpecimens(specimenIds, SpecimenType.Material);
    }


    public async Task<Intervention[]> GetInterventions(IEnumerable<int> ids, SpecimenType type)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await GetInterventionsQuery(dbContext, type)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();
    }

    public async Task<Intervention[]> GetInterventionsForDonors(IEnumerable<int> ids, SpecimenType type)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids, type);

        return await GetInterventions(specimenIds, type);
    }

    public async Task<Intervention[]> GetInterventionsForImages(IEnumerable<int> ids)
    {
        var specimenIds = await _imagesRepository.GetRelatedSpecimens(ids);

        return await GetInterventions(specimenIds, SpecimenType.Material);
    }


    private static IQueryable<Specimen> CreateSpecimensQuery(DomainDbContext dbContext, SpecimenType type)
    {
        var query = dbContext.Set<Specimen>().AsNoTracking();

        if (type == SpecimenType.Material)
            query = query.Include(entity => entity.Material);
        else if (type == SpecimenType.Line)
            query = query.Include(entity => entity.Line);
        else if (type == SpecimenType.Organoid)
            query = query.Include(entity => entity.Organoid);
        else if (type == SpecimenType.Xenograft)
            query = query.Include(entity => entity.Xenograft);

        return query
            .Include(entity => entity.Donor)
            .Include(entity => entity.MolecularData)
            .Where(entity => entity.TypeId == type);
    }

    public static IQueryable<Intervention> GetInterventionsQuery(DomainDbContext dbContext, SpecimenType type)
    {
        return dbContext.Set<Intervention>()
            .AsNoTracking()
            .Include(entity => entity.Specimen.Donor)
            .Where(entity => entity.Specimen.TypeId == type);
    }
}
