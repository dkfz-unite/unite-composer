using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Data.Services;

namespace Unite.Composer.Web.Services;

public class AnalysisTaskService
{
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;


    public AnalysisTaskService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public string Create<T>(string key, T data, AnalysisTaskType type) where T : class
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var task = new Unite.Data.Entities.Tasks.Task
        {
            AnalysisTypeId = type,
            Target = key,
            Data = JsonSerializer.Serialize(data),
            Date = DateTime.UtcNow,
        };

        dbContext.Add(task);
        dbContext.SaveChanges();

        return task.Target;
    }

    public Unite.Data.Entities.Tasks.Task Get(string key)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var task = dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .FirstOrDefault(task => task.Target == key);

        return task;
    }

    public Unite.Data.Entities.Tasks.Task Find(TaskStatusType? status)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return dbContext.Tasks
            .Where(task => task.AnalysisTypeId != null)
            .Where(task => task.StatusTypeId == status)
            .OrderBy(task => task.Date)
            .FirstOrDefault();
    }

    public void Update(Unite.Data.Entities.Tasks.Task task, TaskStatusType? status)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        task.StatusTypeId = status;

        dbContext.Update(task);
        dbContext.SaveChanges();
    }

    public void Delete (Unite.Data.Entities.Tasks.Task task)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.Remove(task);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Iterates analysis tasks of specified type updating their status.
    /// </summary>
    /// <param name="startStatus">Status of the tasks to iterate.</param>
    /// <param name="actionStatus">Status of the tasks being processed.</param>
    /// <param name="endStatus">Status of completed task.</param>
    /// <param name="tasksLimit">Maximum number of concurrent tasks.</param>
    /// <param name="tasksInterval">Milliseconds to wait if tasks limit exceeded.</param>
    /// <param name="handler">Task handler. Completion codes: 1 - success, 2 - rejected, 3 - failed.</param>
    public void Iterate(
        TaskStatusType? startStatus,
        TaskStatusType? actionStatus,
        TaskStatusType? endStatus,
        int tasksLimit,
        int tasksInterval,
        Func<Unite.Data.Entities.Tasks.Task, Task<byte>> handler)
    {
        var jobs = new Dictionary<long, Task>();

        while (true)
        {
            if (jobs.Count >= tasksLimit)
            {
                Thread.Sleep(tasksInterval);

                continue;
            }

            var task = Find(startStatus);

            if (task != null)
            {
                Update(task, actionStatus);

                var job = handler.Invoke(task);

                job.ContinueWith(job => 
                {
                    if (job.Result == 1)
                        Update(task, endStatus);
                    else if (job.Result == 2)
                        Update(task, startStatus);
                    else if (job.Result == 3)
                        Update(task, TaskStatusType.Failed);

                });

                jobs.Add(task.Id, job);
            }
            else
            {
                return;
            }
        }
    }
}
