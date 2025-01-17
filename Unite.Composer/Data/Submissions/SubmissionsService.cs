using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Submissions;

public record SubmissionStatus(string Status, string Comment);

public class SubmissionsService
{
    private readonly DomainDbContext _dbContext;


    public SubmissionsService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<SubmissionStatus> GetStatus(long id)
    {
        var task = await _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .AsNoTracking()
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task != null)
        {
            return new SubmissionStatus(task.StatusTypeId.Value.ToDefinitionString(), task.Comment);
        }
        else
        {
            return null;
        }
    }
}
