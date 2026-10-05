using Microsoft.EntityFrameworkCore;
using Unite.Composer.Clients.DonorFeed;
using Unite.Composer.Clients.Identity;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Data.Entities.Donors;

namespace Unite.Composer.Data.Projects;

public class ProjectService
{
    private readonly DataUserRepository _dataUserRepository;
    private readonly ProjectsRepository _projectRepository;
    private readonly DonorFeedApiClient _donorFeedApiClient;
    private readonly IdentityServiceApiClient _identityServiceApiClient;
    
    public ProjectService(IDbContextFactory<DomainDbContext> dbContextFactory, 
        DonorFeedApiClient donorFeedApiClient, 
        IdentityServiceApiClient identityServiceApiClient)
    {
        _donorFeedApiClient = donorFeedApiClient;
        _identityServiceApiClient = identityServiceApiClient;
        _projectRepository = new ProjectsRepository(dbContextFactory);
        _dataUserRepository = new DataUserRepository(dbContextFactory);
    }
    
    public async Task<List<ProjectUser>> AssignUserToProject(int[] userIds, int projectId)
    {
        var dataUsers = await _dataUserRepository.LoadOrCreate(userIds);
        if (dataUsers == null)
            throw new KeyNotFoundException("Cannot load or create DataUser");
        
        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new KeyNotFoundException("Cannot load Project");
        
        var projectUser = await _projectRepository.AssignToProject(dataUsers.Select(x => x.Id).ToArray(), project.Id);

        await _donorFeedApiClient.IndexProjects();
        
        return projectUser;
    }
    
    public async Task RemoveUserFromProject(int[] userIds, int projectId)
    {
        var dataUsers = await _dataUserRepository.Load(userIds);
        if (dataUsers == null)
            throw new KeyNotFoundException("Cannot load or create DataUser");

        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new KeyNotFoundException("Cannot load Project");
        
        await _projectRepository.RemoveFromProject(dataUsers.Select(x => x.Id).ToArray(), project.Id);
        
        await _donorFeedApiClient.IndexProjects();
    }

    public async Task<List<ProjectUserModel>> ListProjectUsers(int projectId)
    {
        var users = await _identityServiceApiClient.GetUsers();

        var projectUserIds = await _projectRepository.GetRelatedUsers([projectId]);
        var dataUsers = await _dataUserRepository.LoadAll();
        dataUsers = dataUsers.Where(x => projectUserIds.Contains(x.Id)).ToList();

        var projectUsers = new List<ProjectUserModel>();
        
        foreach (var dataUser in dataUsers)
        {
            var user = users.FirstOrDefault(x => x.Id == dataUser.UserId);
            projectUsers.Add(new ProjectUserModel
            {
                UserId = dataUser.UserId,
                DataUserId =  dataUser.Id,
                Email = user?.Email,
                ProjectId =  projectId
            });
        }
        
        return projectUsers;
    }

    public async Task SetIsPublic(int projectId, bool isPublic)
    {
        var project = await _projectRepository.Load(projectId);
        if (project == null)
            throw new KeyNotFoundException("Cannot load Project");
        
        project.IsPublic = isPublic;
        
        await _projectRepository.Save(project);
        
        await _donorFeedApiClient.IndexProjects();
    }
}