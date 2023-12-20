using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Models;
using Unite.Data.Context;

namespace Unite.Composer.Data.Genome;

public class SsmDataService
{
    private readonly DomainDbContext _dbContext;


    public SsmDataService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Retrieves protein coding transcripts affected by given mutation (SSM).
    /// </summary>
    /// <param name="id">Mutation (SSM) identifier.</param>
    /// <returns>Array of transcripts.</returns>    
    public async Task<Transcript[]> GetTranslations(long id)
    {
        var entities = await _dbContext.Set<Unite.Data.Entities.Genome.Variants.SSM.AffectedTranscript>()
            .AsNoTracking()
            .Include(affectedTranscript => affectedTranscript.Feature.Protein)
            .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
            .Where(affectedTranscript => affectedTranscript.VariantId == id)
            .Select(affectedTranscript => affectedTranscript.Feature)
            .ToArrayAsync();

        return entities
            .DistinctBy(entity => entity.Id)
            .Select(entity => new Transcript(entity))
            .ToArray();
    }
}
