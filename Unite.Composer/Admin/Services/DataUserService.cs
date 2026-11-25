using Microsoft.EntityFrameworkCore;
using Unite.Composer.Clients.DonorFeed;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Data.Entities;
using Unite.Data.Entities.Donors;

namespace Unite.Composer.Admin.Services;

public class DataUserService
{
    private readonly DataUserRepository _dataUserRepository;
    private readonly DonorFeedApiClient _donorFeedApiClient;
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    
    public DataUserService(IDbContextFactory<DomainDbContext> dbContextFactory, DonorFeedApiClient donorFeedApiClient)
    {
        _dbContextFactory = dbContextFactory;
        _donorFeedApiClient = donorFeedApiClient;
        _dataUserRepository = new DataUserRepository(dbContextFactory);
    }
    
    public async Task DeleteDataUser(int id, int[] userIds)
    {
        var dataUsers = await _dataUserRepository.Load(userIds);
        if (dataUsers == null)
            throw new Exception("Cannot load data users");

        var dataUserIds = dataUsers.Select(x => x.Id).ToArray();

        var projectIds = await LoadProjectIds(dataUserIds);

        await DeleteDataUsers(dataUserIds);

        foreach (var projectId in projectIds)
            await _donorFeedApiClient.IndexProject(projectId);
    }

    private async Task<int[]> LoadProjectIds(int[] dataUserIds)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Set<ProjectUser>()
            .AsNoTracking()
            .Where(projectUser => dataUserIds.Contains(projectUser.UserId))
            .Select(projectUser => projectUser.ProjectId)
            .Distinct()
            .ToArrayAsync();
    }
    
    private async Task DeleteDataUsers(int[] dataUserIds)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dataUsers = await dbContext.Set<DataUser>()
            .AsNoTracking()
            .Where(dataUser => dataUserIds.Contains(dataUser.Id))
            .ToArrayAsync();

        if (dataUsers.Length > 0)
        {
            dbContext.DataUsers.RemoveRange(dataUsers);
            await dbContext.SaveChangesAsync();
        }
    }
}