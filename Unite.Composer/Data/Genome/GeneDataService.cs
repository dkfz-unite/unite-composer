using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Models;
using Unite.Data.Services;

namespace Unite.Composer.Data.Genome;

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
    public Transcript[] GetTranslations(int id)
    {
        var entities = _dbContext.Set<Unite.Data.Entities.Genome.Variants.SSM.AffectedTranscript>()
            .Include(affectedTranscript => affectedTranscript.Feature)
                .ThenInclude(transcript => transcript.Protein)
            .Where(affectedTranscript => affectedTranscript.Feature.Protein != null)
            .Where(affectedTranscript => affectedTranscript.Feature.GeneId == id)
            .Select(affectedTranscript => affectedTranscript.Feature)
            .ToArray();

        var models = entities
            .DistinctBy(entity => entity.Id)
            .Select(entity => new Transcript(entity))
            .ToArray();

        return models;
    }
}
