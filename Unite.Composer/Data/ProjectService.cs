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
    
    public async Task<ProjectUser> AssignUserToProject(int userId, int projectId)
    {
        var dataUser = await _dataUserRepository.LoadOrCreate(userId);
        if (dataUser == null)
            throw new Exception("Cannot load or create DataUser");

        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new Exception("Cannot load Project");
        
        var projectUser = await _projectRepository.AssignToProject(dataUser.Id, project.Id);

        await _donorFeedApiClient.IndexProjects();
        
        return projectUser;
    }
    
    public async Task RemoveUserFromProject(int userId, int projectId)
    {
        var dataUser = await _dataUserRepository.Load(userId);
        if (dataUser == null)
            throw new Exception("Cannot load or create DataUser");

        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new Exception("Cannot load Project");
        
        await _projectRepository.RemoveFromProject(dataUser.Id, project.Id);
        
        await _donorFeedApiClient.IndexProjects();
    }
}