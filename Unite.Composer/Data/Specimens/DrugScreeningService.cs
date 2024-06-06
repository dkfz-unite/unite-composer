using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Specimens.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Specimens.Analysis.Drugs;

namespace Unite.Composer.Data.Specimens;

public class DrugScreeningService
{
    private readonly DomainDbContext _dbContext;


    public DrugScreeningService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<DrugScreeningModel>> GetAll(int specimenId)
    {
        var screenings = await _dbContext.Set<DrugScreening>()
            .AsNoTracking()
            .Include(screening => screening.Entity)
            .Where(screening => screening.Sample.SpecimenId == specimenId)
            .ToArrayAsync();

        return screenings.Select(screening =>
        {
            return new DrugScreeningModel
            {
                Drug = screening.Entity.Name,
                Gof = screening.Gof,
                Dss = screening.Dss,
                DssS = screening.DssS,
                MinDose = screening.MinDose,
                MaxDose = screening.MaxDose,
                Dose25 = screening.Dose25,
                Dose50 = screening.Dose50,
                Dose75 = screening.Dose75,
                Doses = screening.Doses,
                Responses = screening.Responses
            };
        });
    }
}
