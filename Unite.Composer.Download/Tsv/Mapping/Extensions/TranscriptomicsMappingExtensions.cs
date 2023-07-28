using System.Linq.Expressions;
using Unite.Composer.Download.Extensions;
using Unite.Data.Entities.Genome.Analysis;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class TranscriptomicsMappingExtensions
{
    internal static ClassMap<GeneExpression> MapGeneExpressions(this ClassMap<GeneExpression> map)
    {
        return map
            .MapAnalysedSample(entity => entity.AnalysedSample)
            .Map(entity => entity.Gene.StableId, "gene_id")
            .Map(entity => entity.Gene.Symbol, "gene_symbol")
            .Map(entity => entity.Reads, "reads")
            .Map(entity => entity.TPM, "tpm")
            .Map(entity => entity.FPKM, "fpkm");
    }


    private static ClassMap<T> MapAnalysedSample<T>(this ClassMap<T> map, Expression<Func<T, AnalysedSample>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.Sample.Specimen.Donor.ReferenceId), "donor_id")
            .Map(path.Join(entity => entity.Sample.Specimen.ReferenceId), "specimen_id")
            .Map(path.Join(entity => entity.Sample.Specimen.Type), "specimen_type")
            .Map(path.Join(entity => entity.Sample.ReferenceId), "sample_id");
    }
}
