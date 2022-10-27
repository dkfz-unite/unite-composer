using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public abstract class SpecimenCriteriaBase
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

    public string[] MgmtStatus { get; set; }
    public string[] IdhStatus { get; set; }
    public string[] IdhMutation { get; set; }
    public string[] GeneExpressionSubtype { get; set; }
    public string[] MethylationSubtype { get; set; }
    public bool? GcimpMethylation { get; set; }

    public string[] Drug { get; set; }
    public Range<double?> Dss { get; set; }
    public Range<double?> DssSelective { get; set; }
}
