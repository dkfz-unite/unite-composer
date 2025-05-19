namespace Unite.Composer.Data.Omics.Models.Analysis;

public record AnalysedSpecimen
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }
}
