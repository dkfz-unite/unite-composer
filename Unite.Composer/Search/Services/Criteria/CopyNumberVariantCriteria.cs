namespace Unite.Composer.Search.Services.Criteria;

public class CopyNumberVariantCriteria : VariantCriteriaBase
{
    public string[] SvType { get; set; }
    public string[] CnaType { get; set; }
    public bool? Loh { get; set; }
    public bool? HomoDel { get; set; }
}
