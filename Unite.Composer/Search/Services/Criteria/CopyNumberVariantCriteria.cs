namespace Unite.Composer.Search.Services.Criteria;

public class CopyNumberVariantCriteria : VariantCriteriaBase
{
    public string[] CnaType { get; set; }
    public bool? Loh { get; set; }
    public bool? HomoDel { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || CnaType?.Any() == true
            || Loh != null
            || HomoDel != null;
    }
}
