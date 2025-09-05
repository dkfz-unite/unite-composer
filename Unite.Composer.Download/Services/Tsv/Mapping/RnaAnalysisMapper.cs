using Unite.Data.Entities.Omics.Analysis.Rna;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class RnaAnalysisMapper
{
    public static ClassMap<GeneExpression> GetExpressionMap()
    {
        return new ClassMap<GeneExpression>().MapExpressions();
    }

    public static ClassMap<GeneExpression> MapExpressions(this ClassMap<GeneExpression> map)
    {
        return map
            .Map(entity => entity.Entity.StableId, "gene_id")
            .Map(entity => entity.Entity.Symbol, "gene_symbol")
            .Map(entity => entity.Reads, "reads")
            .Map(entity => entity.TPM, "tpm")
            .Map(entity => entity.FPKM, "fpkm");
    }
}
