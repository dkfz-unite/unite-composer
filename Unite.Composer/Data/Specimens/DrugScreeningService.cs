using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Specimens.Models;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;

namespace Unite.Composer.Data.Specimens;

public class DrugScreeningService
{
    private readonly DomainDbContext _dbContext;


    public DrugScreeningService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public IEnumerable<DrugScreeningModel> GetAll(int specimenId)
    {
        var screenings = _dbContext.Set<DrugScreening>()
            .Include(screening => screening.Drug)
            .Where(screening => screening.SpecimenId == specimenId)
            .ToArray();

        foreach (var screening in screenings)
        {
            yield return new DrugScreeningModel
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
                Concentrations = screening.Concentrations,
                Inhibitions = screening.Inhibitions,
                InhibitionsControl = screening.InhibitionsControl,
                InhibitionsSample = screening.InhibitionsSample
            };
        }
    }
}
