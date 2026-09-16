using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Data.Entities.Donors;

namespace Unite.Composer.Data;

public class ProjectService
{
    private readonly DataUserRepository _dataUserRepository;
    private readonly ProjectsRepository _projectRepository;
    
    public ProjectService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
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

        //TODO: trigger reindexing Projects
        
        return await _projectRepository.AssignToProject(dataUser.Id, project.Id);
    }
}