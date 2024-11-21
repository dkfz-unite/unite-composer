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
    
    public IEnumerable<SubmissionTaskModel> GetAll()
    {
        var tasks = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .OrderBy(task => task.Date)
            .Where(task=> task.StatusTypeId == TaskStatusType.Preparing)
            .ToArray();

        foreach (var task in tasks)
        {
            var taskModel = new SubmissionTaskModel
            {
                Id = task.Id,
                Date = task.Date.ToShortDateString(),
                Target = task.Target,
                Type = task.SubmissionTypeId.ToString()
            };


            yield return taskModel;
        }
    }

    public IEnumerable<SubmissionTaskModel> GetSubmissions(SubmissionTaskType submissionTaskType)
    {
        var tasks = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .OrderBy(task => task.Date)
            .Where(task=> task.StatusTypeId == TaskStatusType.Preparing && task.SubmissionTypeId == submissionTaskType)
            .ToArray();

        foreach (var task in tasks)
        {
            var taskModel = new SubmissionTaskModel
            {
                Id = task.Id,
                Date = task.Date.ToShortDateString(),
                Target = task.Target,
                Comment = task.Comment
            };

            yield return taskModel;
        }
    }

    public bool UpdateSubmissionToPrepared(string id)
    {
        bool successStatus = true;
        try
        {
            var task = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .First(task => task.Id == Convert.ToInt64(id));

            task.StatusTypeId = TaskStatusType.Prepared;

            _dbContext.Update(task);
            _dbContext.SaveChanges();
        }
        catch
        {
            successStatus = false;
        }
        return successStatus;
	}

    public bool UpdateRejectReason(string id, string rejectReason)
    {
        bool successStatus = true;
        try
        {
            var task = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .First(task => task.Id == Convert.ToInt64(id));

            task.Comment = rejectReason;
            task.StatusTypeId = TaskStatusType.Rejected;

            _dbContext.Update(task);
            _dbContext.SaveChanges();
        }
        catch
        {
            successStatus = false;
        }
        return successStatus;
	}

    public Unite.Data.Entities.Tasks.Task FindTaskStatus(string id)
    {
       try{
         var task = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
        .AsNoTracking()
        .First(task => task.Id == Convert.ToInt64(id));
        
        return  task;
       }
       catch{
        return null;
       }
	}
}

