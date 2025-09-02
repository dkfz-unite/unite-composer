using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Context.Repositories.Extensions.Queryable;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;

namespace Unite.Composer.Download.Repositories;

public class DonorsDataRepository : DataRepository
{
    public DonorsDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<Donor[]> GetDonors(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateDonorsQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();
    }

    public async Task<Treatment[]> GetTreatments(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateTreatmentsQuery(dbContext)
            .Where(entity => ids.Contains(entity.DonorId))
            .ToArrayAsync();
    }

    public async Task<Donor[]> GetDonorsForImages(IEnumerable<int> ids)
    {
        var donorIds = await _imagesRepository.GetRelatedDonors(ids);

        return await GetDonors(donorIds);
    }

    public async Task<Treatment[]> GetTreatmentsForImages(IEnumerable<int> ids)
    {
        var donorIds = await _imagesRepository.GetRelatedDonors(ids);

        return await GetTreatments(donorIds);
    }

    public async Task<Donor[]> GetDonorsForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await _specimensRepository.GetRelatedDonors(ids);

        return await GetDonors(donorIds);
    }

    public async Task<Treatment[]> GetTreatmentsForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await _specimensRepository.GetRelatedDonors(ids);

        return await GetTreatments(donorIds);
    }


    private static IQueryable<Donor> CreateDonorsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Donor>().AsNoTracking()
            .IncludeClinicalData()
            .IncludeProjects()
            .IncludeStudies();
    }

    private static IQueryable<Treatment> CreateTreatmentsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Treatment>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.Therapy);
    }
}
