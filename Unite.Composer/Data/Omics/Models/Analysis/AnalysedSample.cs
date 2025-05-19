using Unite.Data.Entities.Omics.Analysis.Enums;

namespace Unite.Composer.Data.Omics.Models.Analysis;

public record AnalysedSample
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public double? Ploidy { get; set; }
    public double? Purity { get; set; }
    public AnalysisType[] Analyses { get; set; }

    public AnalysedSpecimen Specimen { get; set; }
}
