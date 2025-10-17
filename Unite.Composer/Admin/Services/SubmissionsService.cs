using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Admin.Services;

public record Submission(long Id, SubmissionTaskType Type, DateTime Date);

public class SubmissionsService
{
    private readonly DomainDbContext _dbContext;

    public SubmissionsService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Submission[]> GetPedning(long[] ids = null)
    {
        var query = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .OrderBy(task => task.Id)
            .Where(task => task.SubmissionTypeId != null)
            .Where(task => task.StatusTypeId == TaskStatusType.Preparing);

        if (ids.IsNotEmpty())
            query = query.Where(task => ids.Contains(task.Id));
    
        var tasks = await query.ToArrayAsync();

        return tasks.Select(task => new Submission(task.Id, task.SubmissionTypeId.Value, task.Date)).ToArray();
    }

    public async Task<bool> Approve(long id)
    {
        var task = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task != null)
        {
            task.StatusTypeId = TaskStatusType.Prepared;

            _dbContext.Update(task);
            _dbContext.SaveChanges();

            return true;
        }
        else
        {
            return false;
        }
	}

    public async Task<bool> Reject(long id, string reason)
    {
        var task = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task != null)
        {
            task.StatusTypeId = TaskStatusType.Rejected;
            task.Comment = reason;

            _dbContext.Update(task);
            _dbContext.SaveChanges();

            return true;
        }
        else
        {
            return false;
        }
	}
}
