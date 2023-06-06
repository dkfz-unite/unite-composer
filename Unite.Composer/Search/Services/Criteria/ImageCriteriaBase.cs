namespace Unite.Composer.Search.Services.Criteria;

public record ImageCriteriaBase
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasGeneExp { get; set; }
}
