using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Models;
using Unite.Data.Context;

namespace Unite.Composer.Data.Genome;

public class SmDataService
{
    private readonly DomainDbContext _dbContext;


    public SmDataService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Retrieves protein coding transcripts affected by given mutation (SM).
    /// </summary>
    /// <param name="id">Mutation (SM) identifier.</param>
    /// <returns>Array of transcripts.</returns>    
    public async Task<Transcript[]> GetTranslations(int id)
    {
        var entities = await _dbContext.Set<Unite.Data.Entities.Genome.Analysis.Dna.Sm.AffectedTranscript>()
            .AsNoTracking()
            .Include(affectedTranscript => affectedTranscript.Feature.Protein)
            .Where(affectedTranscript => affectedTranscript.ProteinChange != null)
            .Where(affectedTranscript => affectedTranscript.VariantId == id)
            .Select(affectedTranscript => affectedTranscript.Feature)
            .ToArrayAsync();

        return entities
            .DistinctBy(entity => entity.Id)
            .Select(entity => new Transcript(entity))
            .ToArray();
    }
}
