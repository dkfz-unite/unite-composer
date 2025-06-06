using System.Linq.Expressions;
using Unite.Data.Entities.Omics.Analysis;
using Unite.Data.Entities.Omics.Analysis.Rna;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class TranscriptomicsMappingExtensions
{
    public static ClassMap<GeneExpression> MapExpressions(this ClassMap<GeneExpression> map)
    {
        return map
            .MapSample(entity => entity.Sample)
            .Map(entity => entity.Entity.StableId, "gene_id")
            .Map(entity => entity.Entity.Symbol, "gene_symbol")
            .Map(entity => entity.Reads, "reads")
            .Map(entity => entity.TPM, "tpm")
            .Map(entity => entity.FPKM, "fpkm");
    }


    private static ClassMap<T> MapSample<T>(this ClassMap<T> map, Expression<Func<T, Sample>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.Specimen.Donor.ReferenceId), "donor_id")
            .Map(path.Join(entity => entity.Specimen.ReferenceId), "specimen_id")
            .Map(path.Join(entity => entity.Specimen.TypeId), "specimen_type")
            .Map(path.Join(entity => entity.Specimen.ReferenceId), "sample_id");
    }
}
