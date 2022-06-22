namespace Unite.Composer.Search.Services.Filters.Constants;

public static class CellLineFilterNames
{
    private const string _prefix = "CellLine";


    public static readonly string ReferenceId = $"{_prefix}.ReferenceId";
    public static readonly string Species = $"{_prefix}.Species";
    public static readonly string Type = $"{_prefix}.Type";
    public static readonly string CultureType = $"{_prefix}.CultureType";

    public static readonly string Name = $"{_prefix}.Name";

    public static readonly string MgmtStatus = $"{_prefix}.MgmtStatus";
    public static readonly string IdhStatus = $"{_prefix}.IhdStatus";
    public static readonly string IdhMutation = $"{_prefix}.IdhMutation";
    public static readonly string GeneExpressionSubtype = $"{_prefix}.GeneExpressionSubtype";
    public static readonly string MethylationSubtype = $"{_prefix}.MethylationSubtype";
    public static readonly string GcimpMethylation = $"{_prefix}.GcimpMethylation";
}
