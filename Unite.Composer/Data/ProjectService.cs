using Microsoft.EntityFrameworkCore;
using Unite.Composer.Clients.DonorFeed;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Data.Entities.Donors;

namespace Unite.Composer.Data;

public class ProjectService
{
    private readonly DataUserRepository _dataUserRepository;
    private readonly ProjectsRepository _projectRepository;
    private readonly DonorFeedApiClient _donorFeedApiClient;
    
    public ProjectService(IDbContextFactory<DomainDbContext> dbContextFactory, DonorFeedApiClient donorFeedApiClient)
    {
        _donorFeedApiClient = donorFeedApiClient;
        _projectRepository = new ProjectsRepository(dbContextFactory);
        _dataUserRepository = new DataUserRepository(dbContextFactory);
    }
    
    public async Task<List<ProjectUser>> AssignUserToProject(int[] userIds, int projectId)
    {
        var dataUsers = await _dataUserRepository.LoadOrCreate(userIds);
        if (dataUsers == null)
            throw new Exception("Cannot load or create DataUser");

        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new Exception("Cannot load Project");
        
        var projectUser = await _projectRepository.AssignToProject(dataUsers.Select(x => x.Id).ToArray(), project.Id);

        await _donorFeedApiClient.IndexProjects();
        
        return projectUser;
    }
    
    public async Task RemoveUserFromProject(int[] userIds, int projectId)
    {
        var dataUsers = await _dataUserRepository.Load(userIds);
        if (dataUsers == null)
            throw new Exception("Cannot load or create DataUser");

        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new Exception("Cannot load Project");
        
        await _projectRepository.RemoveFromProject(dataUsers.Select(x => x.Id).ToArray(), project.Id);
        
        await _donorFeedApiClient.IndexProjects();
    }
}