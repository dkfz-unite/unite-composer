using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class MolecularDataResource
{
    public string MgmtStatus { get; set; }
    public string IdhStatus { get; set; }
    public string IdhMutation { get; set; }
    public string GeneExpressionSubtype { get; set; }
    public string MethylationSubtype { get; set; }
    public bool? GcimpMethylation { get; set; }
    public string[] GeneKnockouts { get; set; }


    public MolecularDataResource(MolecularDataIndex index)
    {
        MgmtStatus = index.MgmtStatus;
        IdhStatus = index.IdhStatus;
        IdhMutation = index.IdhMutation;
        GeneExpressionSubtype = index.GeneExpressionSubtype;
        MethylationSubtype = index.MethylationSubtype;
        GcimpMethylation = index.GcimpMethylation;
        GeneKnockouts = index.GeneKnockouts;
    }
}
