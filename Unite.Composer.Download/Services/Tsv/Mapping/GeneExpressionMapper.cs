using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Omics.Analysis.Rna;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class GeneExpressionMapper
{
    public static ClassMap<GeneExpression> GetGeneExpressionMap()
    {
        return new ClassMap<GeneExpression>().MapExpressions();
    }
}
