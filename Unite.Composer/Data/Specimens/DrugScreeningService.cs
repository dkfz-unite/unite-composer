using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Specimens.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Specimens;

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
            .Include(screening => screening.Drug)
            .Where(screening => screening.SpecimenId == specimenId)
            .ToArrayAsync();

        return screenings.Select(screening =>
        {
            return new DrugScreeningModel
            {
                Drug = screening.Drug.Name,
                Dss = screening.Dss,
                DssSelective = screening.DssSelective,
                Gof = screening.Gof,
                MinConcentration = screening.MinConcentration,
                MaxConcentration = screening.MaxConcentration,
                AbsIC25 = screening.AbsIC25,
                AbsIC50 = screening.AbsIC50,
                AbsIC75 = screening.AbsIC75,
                Concentration = screening.Concentration,
                Inhibition = screening.Inhibition,
                Dose = screening.InhibitionLine,
                Response = screening.ConcentrationLine
            };
        });
    }
}
