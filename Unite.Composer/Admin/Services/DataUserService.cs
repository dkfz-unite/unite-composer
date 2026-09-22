using Microsoft.EntityFrameworkCore;
using Unite.Composer.Clients.DonorFeed;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;

namespace Unite.Composer.Admin.Services;

public class DataUserService
{
    private readonly DataUserRepository _dataUserRepository;
    private readonly DonorFeedApiClient _donorFeedApiClient;
    
    public DataUserService(IDbContextFactory<DomainDbContext> dbContextFactory, DonorFeedApiClient donorFeedApiClient)
    {
        _donorFeedApiClient = donorFeedApiClient;
        _dataUserRepository = new DataUserRepository(dbContextFactory);
    }
    
    public async Task DeleteDataUser(int id, int[] userIds)
    {
        var dataUsers = await _dataUserRepository.Load(userIds);
        if (dataUsers == null)
            throw new Exception("Cannot load data users");

        await _dataUserRepository.Delete(dataUsers.Select(x => x.Id).ToArray());
        
        await _donorFeedApiClient.IndexProjects();
    }
}