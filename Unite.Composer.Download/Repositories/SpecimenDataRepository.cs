using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Analysis.Drugs;

namespace Unite.Composer.Download.Repositories;

public class SpecimenDataRepository : DataRepository
{
    public SpecimenDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<Specimen[]> GetSpecimens(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateSpecimensQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();
    }

    public async Task<Specimen[]> GetSpecimensForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids);

        return await GetSpecimens(specimenIds);
    }

    public async Task<Specimen[]> GetSpecimensForImages(IEnumerable<int> ids)
    {
        var specimenIds = await _imagesRepository.GetRelatedSpecimens(ids);

        return await GetSpecimens(specimenIds);
    }


    public async Task<Intervention[]> GetInterventions(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await GetInterventionsQuery(dbContext)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();
    }

    public async Task<Intervention[]> GetInterventionsForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids);

        return await GetInterventions(specimenIds);
    }

    public async Task<Intervention[]> GetInterventionsForImages(IEnumerable<int> ids)
    {
        var specimenIds = await _imagesRepository.GetRelatedSpecimens(ids);

        return await GetInterventions(specimenIds);
    }


    private static IQueryable<Specimen> CreateSpecimensQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Specimen>()
            .AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.Material)
            .Include(entity => entity.Line)
            .Include(entity => entity.Organoid)
            .Include(entity => entity.Xenograft)
            .Include(entity => entity.MolecularData);
    }

    public static IQueryable<Intervention> GetInterventionsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Intervention>()
            .AsNoTracking()
            .Include(entity => entity.Specimen.Donor);
    }

    public static IQueryable<DrugScreening> CreateDrugScreeningsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<DrugScreening>()
            .AsNoTracking()
            .Include(entity => entity.Sample.Specimen)
            .Include(entity => entity.Entity);
    }
}
