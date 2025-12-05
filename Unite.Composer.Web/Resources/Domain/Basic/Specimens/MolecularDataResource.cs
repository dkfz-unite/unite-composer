using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class MolecularDataResource
{
    public bool? MgmtStatus { get; set; }
    public bool? IdhStatus { get; set; }
    public string IdhMutation { get; set; }
    public bool? TertStatus { get; set; }
    public string TertMutation { get; set; }
    public string ExpressionSubtype { get; set; }
    public string MethylationSubtype { get; set; }
    public bool? GcimpMethylation { get; set; }
    public string[] GeneKnockouts { get; set; }


    public MolecularDataResource(MolecularDataIndex index)
    {
        MgmtStatus = index.MgmtStatus;
        IdhStatus = index.IdhStatus;
        IdhMutation = index.IdhMutation;
        TertStatus = index.TertStatus;
        TertMutation = index.TertMutation;
        ExpressionSubtype = index.ExpressionSubtype;
        MethylationSubtype = index.MethylationSubtype;
        GcimpMethylation = index.GcimpMethylation;
        GeneKnockouts = index.GeneKnockouts;
    }
}
