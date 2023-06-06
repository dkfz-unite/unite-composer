namespace Unite.Composer.Search.Services.Filters.Constants;

public class SpecimenFilterNames
{
    private const string _prefix = "Specimen";


    public static readonly string Id = $"{_prefix}.Id";
    public static readonly string Type = $"{_prefix}.Type";

    public static readonly string MgmtStatus = $"{_prefix}.MgmtStatus";
    public static readonly string IdhStatus = $"{_prefix}.IhdStatus";
    public static readonly string IdhMutation = $"{_prefix}.IdhMutation";
    public static readonly string GeneExpressionSubtype = $"{_prefix}.GeneExpressionSubtype";
    public static readonly string MethylationSubtype = $"{_prefix}.MethylationSubtype";
    public static readonly string GcimpMethylation = $"{_prefix}.GcimpMethylation";

    public static readonly string Drug = $"{_prefix}.Drug";
    public static readonly string Dss = $"{_prefix}.Dss";
    public static readonly string DssSelective = $"{_prefix}.DssSelective";

    public static readonly string HasDrugs = $"{_prefix}.HasDrugs";
    public static readonly string HasSsms = $"{_prefix}.HasSsms";
    public static readonly string HasCnvs = $"{_prefix}.HasCnvs";
    public static readonly string HasSvs = $"{_prefix}.HasSvs";
    public static readonly string HasGeneExp = $"{_prefix}.HasGeneExp";
}
