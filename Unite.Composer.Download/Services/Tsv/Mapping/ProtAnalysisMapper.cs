using Unite.Data.Entities.Omics.Analysis.Prot;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class ProtAnalysisMapper
{
    public static ClassMap<ProteinExpression> GetExpressionMap()
    {
        return new ClassMap<ProteinExpression>().MapExpressions();
    }

    public static ClassMap<ProteinExpression> MapExpressions(this ClassMap<ProteinExpression> map)
    {
        return map
            .Map(entity => entity.Entity.StableId, "protein_id")
            .Map(entity => entity.Entity.AccessionId, "protein_accession_id")
            .Map(entity => entity.Entity.Symbol, "protein_symbol")
            .Map(entity => entity.Intensity, "intensity");
    }
}
