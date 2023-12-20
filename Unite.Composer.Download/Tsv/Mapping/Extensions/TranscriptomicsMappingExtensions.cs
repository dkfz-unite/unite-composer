using System.Linq.Expressions;
using Unite.Data.Entities.Genome.Analysis;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class TranscriptomicsMappingExtensions
{
    public static ClassMap<BulkExpression> MapBulkExpressions(this ClassMap<BulkExpression> map)
    {
        return map
            .MapAnalysedSample(entity => entity.AnalysedSample)
            .Map(entity => entity.Entity.StableId, "gene_id")
            .Map(entity => entity.Entity.Symbol, "gene_symbol")
            .Map(entity => entity.Reads, "reads")
            .Map(entity => entity.TPM, "tpm")
            .Map(entity => entity.FPKM, "fpkm");
    }


    private static ClassMap<T> MapAnalysedSample<T>(this ClassMap<T> map, Expression<Func<T, AnalysedSample>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.TargetSample.Donor.ReferenceId), "donor_id")
            .Map(path.Join(entity => entity.TargetSample.ReferenceId), "specimen_id")
            .Map(path.Join(entity => entity.TargetSample.TypeId), "specimen_type")
            .Map(path.Join(entity => entity.TargetSample.ReferenceId), "sample_id");
    }
}
