namespace Unite.Composer.Search.Services.Filters.Constants;

public static class XenograftFilterNames
{
    private const string _prefix = "Xenograft";


    public static readonly string ReferenceId = $"{_prefix}.ReferenceId";
    public static readonly string MouseStrain = $"{_prefix}.MouseStrain";
    public static readonly string Tumorigenicity = $"{_prefix}.Tumorigenicity";
    public static readonly string TumorGrowthForm = $"{_prefix}.TumorGrowthForm";
    public static readonly string SurvivalDays = $"{_prefix}.SurvivalDays";
    public static readonly string Intervention = $"{_prefix}.Intervention";

    //public static readonly string MgmtStatus = $"{_prefix}.MgmtStatus";
    //public static readonly string IdhStatus = $"{_prefix}.IhdStatus";
    //public static readonly string IdhMutation = $"{_prefix}.IdhMutation";
    //public static readonly string GeneExpressionSubtype = $"{_prefix}.GeneExpressionSubtype";
    //public static readonly string MethylationSubtype = $"{_prefix}.MethylationSubtype";
    //public static readonly string GcimpMethylation = $"{_prefix}.GcimpMethylation";

    //public static readonly string Drug = $"{_prefix}.Drug";
    //public static readonly string Dss = $"{_prefix}.Dss";
    //public static readonly string DssSelective = $"{_prefix}.DssSelective";
}
