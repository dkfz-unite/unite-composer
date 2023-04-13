namespace Unite.Composer.Search.Services.Criteria;

public record MutationCriteria : VariantCriteriaBase
{
    public string[] Type { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Any() == true;
    }
}
