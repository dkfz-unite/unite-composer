using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Composer.Admin.Submissions.Models;  

namespace Unite.Composer.Admin.Submissions;

public class SubmissionService
{
    private readonly DomainDbContext _dbContext;

    public SubmissionService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<SubmissionTaskModel[]> GetAll()
    {
         var tasks = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
        .AsNoTracking()
        .OrderBy(task => task.Date)
        .Where(task => task.StatusTypeId == TaskStatusType.Preparing)
        .ToArrayAsync();

        var taskModels = tasks.Select(task => new SubmissionTaskModel
        {
            Id = task.Id,
            Date = task.Date.ToShortDateString(),
            Target = task.Target,
            Type = task.SubmissionTypeId.ToString()
        }).ToArray();

        return taskModels;
    }

    public async Task<bool> UpdateSubmissionToPrepared(string id)
    {
        bool successStatus = true;
        try
        {
            var task = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .FirstAsync(task => task.Id == Convert.ToInt64(id));

            task.StatusTypeId = TaskStatusType.Prepared;

            _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            successStatus = false;
        }
        return successStatus;
	}

    public async Task<bool> UpdateRejectReason(string id, string rejectReason)
    {
        bool successStatus = true;
        try
        {
            var task = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .FirstAsync(task => task.Id == Convert.ToInt64(id));

            task.Comment = rejectReason;
            task.StatusTypeId = TaskStatusType.Rejected;

            _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            successStatus = false;
        }
        return successStatus;
	}

    public async Task<Unite.Data.Entities.Tasks.Task> FindTaskStatus(string id)
    {
       try{
         var task = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
        .AsNoTracking()
        .FirstAsync(task => task.Id == Convert.ToInt64(id));
        
        return  task;
       }
       catch{
        return null;
       }
	}
}

