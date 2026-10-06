using Microsoft.EntityFrameworkCore;
using Unite.Composer.Clients.DonorFeed;
using Unite.Composer.Clients.Identity;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Data.Entities;
using Unite.Data.Entities.Donors;

namespace Unite.Composer.Data.Projects;

public class ProjectService
{
    private readonly IDbContextFactory<DomainDbContext>  _dbContextFactory;
    private readonly DataUserRepository _dataUserRepository;
    private readonly ProjectsRepository _projectRepository;
    private readonly DonorFeedApiClient _donorFeedApiClient;
    private readonly IdentityServiceApiClient _identityServiceApiClient;
    
    public ProjectService(IDbContextFactory<DomainDbContext> dbContextFactory, 
        DonorFeedApiClient donorFeedApiClient, 
        IdentityServiceApiClient identityServiceApiClient)
    {
        _dbContextFactory = dbContextFactory;
        _donorFeedApiClient = donorFeedApiClient;
        _identityServiceApiClient = identityServiceApiClient;
        _projectRepository = new ProjectsRepository(dbContextFactory);
        _dataUserRepository = new DataUserRepository(dbContextFactory);
    }
    
    public async Task<ProjectUser[]> AssignUserToProject(int[] userIds, int projectId)
    {
        var dataUsers = await LoadOrCreateDataUsers(userIds);
        if (dataUsers == null)
            throw new KeyNotFoundException("Cannot load or create DataUser");
        
        var project = await LoadProject(projectId);
        if (project == null)
            throw new KeyNotFoundException("Cannot load Project");
        
        var projectUser = await AssignToProject(dataUsers.Select(dataUser => dataUser.Id).ToArray(), project.Id);

        await _donorFeedApiClient.IndexProjects();
        
        return projectUser;
    }
    
    public async Task RemoveUserFromProject(int[] userIds, int projectId)
    {
        var dataUsers = await _dataUserRepository.Load(userIds);
        if (dataUsers == null)
            throw new KeyNotFoundException("Cannot load or create DataUser");

        var project = await LoadProject(projectId);
        if (project == null)
            throw new KeyNotFoundException("Cannot load Project");
        
        await RemoveFromProject(dataUsers.Select(dataUser => dataUser.Id).ToArray(), project.Id);
        
        await _donorFeedApiClient.IndexProjects();
    }

    public async Task<ProjectUserModel[]> ListProjectUsers(int projectId)
    {
        var users = await _identityServiceApiClient.GetUsers();

        var projectUserIds = await _projectRepository.GetRelatedUsers([projectId]);
        var dataUsers = await _dataUserRepository.LoadAll();
        dataUsers = dataUsers.Where(dataUser => projectUserIds.Contains(dataUser.Id)).ToList();

        var projectUsers = new List<ProjectUserModel>();
        
        foreach (var dataUser in dataUsers)
        {
            var user = users.FirstOrDefault(userResource => userResource.Id == dataUser.UserId);
            projectUsers.Add(new ProjectUserModel
            {
                UserId = dataUser.UserId,
                DataUserId =  dataUser.Id,
                Email = user?.Email,
                ProjectId =  projectId
            });
        }
        
        return projectUsers.ToArray();
    }

    public async Task SetIsPublic(int projectId, bool isPublic)
    {
        var project = await LoadProject(projectId);
        if (project == null)
            throw new KeyNotFoundException("Cannot load Project");
        
        project.IsPublic = isPublic;
        
        await SaveProject(project);
        
        await _donorFeedApiClient.IndexProjects();
    }
    
    private async Task<Project> LoadProject(int projectId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Projects.FirstOrDefaultAsync(project => project.Id == projectId);
    }

    private async Task SaveProject(Project project)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Set<Project>().Update(project);
        await dbContext.SaveChangesAsync();
    }

    private async Task<ProjectUser[]> AssignToProject(int[] dataUserIds, int projectId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var distinctUserIds = dataUserIds.Distinct().ToArray();

        var existingProjectUsers = await dbContext.Set<ProjectUser>()
            .AsNoTracking()
            .Where(projectUser => projectUser.ProjectId == projectId && distinctUserIds.Contains(projectUser.UserId))
            .ToArrayAsync();

        var existingUserIds = existingProjectUsers.Select(projectUser => projectUser.UserId).ToHashSet();

        var newProjectUsers = distinctUserIds
            .Where(userId => !existingUserIds.Contains(userId))
            .Select(userId => new ProjectUser { ProjectId = projectId, UserId = userId })
            .ToArray();

        if (newProjectUsers.Length > 0)
        {
            dbContext.Set<ProjectUser>().AddRange(newProjectUsers);
            await dbContext.SaveChangesAsync();
        }

        return existingProjectUsers.Concat(newProjectUsers).ToArray();
    }

    private async Task RemoveFromProject(int[] dataUserIds, int projectId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var projectUsers = await dbContext.Set<ProjectUser>()
            .AsNoTracking()
            .Where(projectUser => projectUser.ProjectId == projectId && dataUserIds.Contains(projectUser.UserId))
            .ToListAsync();

        if (projectUsers.Count > 0)
        {
            dbContext.Set<ProjectUser>().RemoveRange(projectUsers);
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task<DataUser[]> LoadOrCreateDataUsers(int[] userIds)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var distinctUserIds = userIds.Distinct().ToArray();

        var existingDataUsers = await dbContext.Set<DataUser>()
            .AsNoTracking()
            .Where(dataUser => distinctUserIds.Contains(dataUser.UserId))
            .ToListAsync();

        var existingUserIds = existingDataUsers.Select(dataUser => dataUser.UserId).ToHashSet();

        var newDataUsers = distinctUserIds
            .Where(userId => !existingUserIds.Contains(userId))
            .Select(userId => new DataUser { UserId = userId })
            .ToList();

        if (newDataUsers.Count > 0)
        {
            dbContext.DataUsers.AddRange(newDataUsers);
            await dbContext.SaveChangesAsync();
        }

        return existingDataUsers.Concat(newDataUsers).ToArray();
    }
}