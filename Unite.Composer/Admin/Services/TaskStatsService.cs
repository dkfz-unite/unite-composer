using Unite.Data.Entities.Tasks.Enums;
using Unite.Data.Services;

namespace Unite.Composer.Admin.Services;

public record TaskNumbersStats (int Submission, int Annotation, int Indexing);

public class TaskStatsService
{
    private readonly DomainDbContext _dbContext;


    public TaskStatsService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public TaskNumbersStats GetTaskNumbersStats()
    {
        var submissionTasksNumber = _dbContext.Set<Unite.Data.Entities.Tasks.Task>().Count(task => task.SubmissionTypeId != null);
        var annotationTasksNumber = _dbContext.Set<Unite.Data.Entities.Tasks.Task>().Count(task => task.AnnotationTypeId != null);
        var indexingTasksNumber = _dbContext.Set<Unite.Data.Entities.Tasks.Task>().Count(task => task.IndexingTypeId != null);

        return new TaskNumbersStats(submissionTasksNumber, annotationTasksNumber, indexingTasksNumber);
    }

    public IDictionary<SubmissionTaskType, int> GetSubmissionTasksStats()
    {
        var tasks = Enum.GetValues<SubmissionTaskType>().ToDictionary(value => value, value => 0);

        foreach (var taskType in tasks.Keys)
        {
            tasks[taskType] = _dbContext.Set<Unite.Data.Entities.Tasks.Task>().Count(task => task.SubmissionTypeId == taskType);
        }

        return tasks;
    }

    public IDictionary<AnnotationTaskType, int> GetAnnotationTasksStats()
    {
        var tasks = Enum.GetValues<AnnotationTaskType>().ToDictionary(value => value, value => 0);

        foreach (var taskType in tasks.Keys)
        {
            tasks[taskType] = _dbContext.Set<Unite.Data.Entities.Tasks.Task>().Count(task => task.AnnotationTypeId == taskType);
        }

        return tasks;
    }

    public IDictionary<IndexingTaskType, int> GetIndexingTasksStats()
    {
        var tasks = Enum.GetValues<IndexingTaskType>().ToDictionary(value => value, value => 0);

        foreach (var taskType in tasks.Keys)
        {
            tasks[taskType] = _dbContext.Set<Unite.Data.Entities.Tasks.Task>().Count(task => task.IndexingTypeId == taskType);
        }

        return tasks;
    }
}
