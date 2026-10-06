using Microsoft.EntityFrameworkCore;
using Unite.Composer.Clients.DonorFeed;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;

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

        await DeleteDataUsers(dataUsers.Select(x => x.Id).ToArray());
        
        await _donorFeedApiClient.IndexProjects();
    }
    
    private async Task DeleteDataUsers(int[] dataUserIds)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dataUsers = await dbContext.DataUsers
            .Where(du => dataUserIds.Contains(du.Id))
            .ToListAsync();

        if (dataUsers.Count > 0)
        {
            dbContext.DataUsers.RemoveRange(dataUsers);
            await dbContext.SaveChangesAsync();
        }
    }
}