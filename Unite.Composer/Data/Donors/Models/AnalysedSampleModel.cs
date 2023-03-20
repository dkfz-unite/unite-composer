using Unite.Data.Entities.Genome.Analysis.Enums;

namespace Unite.Composer.Data.Donors.Models;

public class AnalysedSampleModel
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }
    public AnalysisType[] Analyses { get; set; }
}
