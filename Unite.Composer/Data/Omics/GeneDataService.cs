using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Omics.Models;
using Unite.Data.Context;

namespace Unite.Composer.Data.Omics;

public class GeneDataService
{
    private readonly DomainDbContext _dbContext;


    public GeneDataService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves protein coding transcripts affected by any mutation in given gene.
    /// </summary>
    /// <param name="id">Gene identifier.</param>
    /// <returns>Array of transcripts.</returns>    
    public async Task<Transcript[]> GetTranslations(int id)
    {
        var entities = await _dbContext.Set<Unite.Data.Entities.Omics.Analysis.Dna.Sm.AffectedTranscript>()
            .AsNoTracking()
            .Include(affectedTranscript => affectedTranscript.Feature.Protein)
            .Where(affectedTranscript => affectedTranscript.ProteinChange != null)
            .Where(affectedTranscript => affectedTranscript.Feature.GeneId == id)
            .Select(affectedTranscript => affectedTranscript.Feature)
            .ToArrayAsync();

        return entities
            .DistinctBy(entity => entity.Id)
            .Select(entity => new Transcript(entity))
            .ToArray();
    }
}
