using Unite.Data.Entities.Genome.Transcriptomics;

namespace Unite.Composer.Download.Extensions.Queryable;

public static class GeneExpressionExtensions
{
    public static IQueryable<GeneExpression> FilterByGeneIds(this IQueryable<GeneExpression> query, IEnumerable<int> ids)
    {
        return query.Where(entity => ids.Contains(entity.GeneId));
    }
}
