using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Tasks.Enums;

namespace Unite.Composer.Admin.Services;

public record TaskNumbersStats (int Submission, int Annotation, int Indexing);

public class TaskStatsService
{
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;


    public TaskStatsService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public async Task<TaskNumbersStats> GetTaskNumbersStats()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var submissionTasksNumber = await dbContext.Set<Unite.Data.Entities.Tasks.Task>().CountAsync(task => task.SubmissionTypeId != null && task.StatusTypeId != TaskStatusType.Rejected);
        var annotationTasksNumber = await dbContext.Set<Unite.Data.Entities.Tasks.Task>().CountAsync(task => task.AnnotationTypeId != null && task.StatusTypeId != TaskStatusType.Rejected);
        var indexingTasksNumber = await dbContext.Set<Unite.Data.Entities.Tasks.Task>().CountAsync(task => task.IndexingTypeId != null && task.StatusTypeId != TaskStatusType.Rejected);

        return new TaskNumbersStats(submissionTasksNumber, annotationTasksNumber, indexingTasksNumber);
    }

    public async Task<IDictionary<SubmissionTaskType, int>> GetSubmissionTasksStats()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var tasks = Enum.GetValues<SubmissionTaskType>().ToDictionary(value => value, value => 0);

        foreach (var taskType in tasks.Keys)
        {
            tasks[taskType] = await dbContext.Set<Unite.Data.Entities.Tasks.Task>()
                .Where(task => task.SubmissionTypeId == taskType)
                .Where(task => task.StatusTypeId != TaskStatusType.Rejected)
                .CountAsync();
        }

        return tasks;
    }

    public async Task<IDictionary<AnnotationTaskType, int>> GetAnnotationTasksStats()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var tasks = Enum.GetValues<AnnotationTaskType>().ToDictionary(value => value, value => 0);

        foreach (var taskType in tasks.Keys)
        {
            tasks[taskType] = await dbContext.Set<Unite.Data.Entities.Tasks.Task>()
                .Where(task => task.AnnotationTypeId == taskType)
                .Where(task => task.StatusTypeId != TaskStatusType.Rejected)
                .CountAsync();
        }

        return tasks;
    }

    public async Task<IDictionary<IndexingTaskType, int>> GetIndexingTasksStats()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var tasks = Enum.GetValues<IndexingTaskType>().ToDictionary(value => value, value => 0);

        foreach (var taskType in tasks.Keys)
        {
            tasks[taskType] = await dbContext.Set<Unite.Data.Entities.Tasks.Task>()
                .Where(task => task.IndexingTypeId == taskType)
                .Where(task => task.StatusTypeId != TaskStatusType.Rejected)
                .CountAsync();
        }

        return tasks;
    }

    public async Task<bool> GetStatus(SubmissionTaskType type)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var hasTasks = await dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AnyAsync(task => task.SubmissionTypeId == type);

        return !hasTasks;
    }

    public async Task<bool> GetStatus(AnnotationTaskType type)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var hasTasks = await dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AnyAsync(task => task.AnnotationTypeId == type);

        return !hasTasks;
    }

    public async Task<bool> GetStatus(IndexingTaskType type)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var hasTasks = await dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AnyAsync(task => task.IndexingTypeId == type);

        return !hasTasks;
    }
}
