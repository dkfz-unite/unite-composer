namespace Unite.Composer.Search.Services.Filters.Constants;

public static class GeneFilterNames
{
    private const string _prefix = "Gene";

    public static readonly string Id = $"{_prefix}.Id";
    public static readonly string Symbol = $"{_prefix}.Symbol";
    public static readonly string Biotype = $"{_prefix}.Biotype";
    public static readonly string Chromosome = $"{_prefix}.Chromosome";
    public static readonly string Position = $"{_prefix}.Position";
    public static readonly string HasMutations = $"{_prefix}.HasMutations";
    public static readonly string HasCopyNumberVariants = $"{_prefix}.HasCopyNumberVariants";
    public static readonly string HasStructuralVariants = $"{_prefix}.HasStructuralVariants";
    public static readonly string HasVariants = $"{_prefix}.HasVariants";
    public static readonly string HasExpressions = $"{_prefix}.HasExpressions";
}
