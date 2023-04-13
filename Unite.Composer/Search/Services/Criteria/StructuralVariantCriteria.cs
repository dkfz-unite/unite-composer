namespace Unite.Composer.Search.Services.Criteria;

public record StructuralVariantCriteria : VariantCriteriaBase
{
    public string[] Type { get; set; }
    public bool? Inverted { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Any() == true
            || Inverted != null;
    }
}
