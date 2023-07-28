using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GenesDataResource : GeneDataResource
{
    public int Total { get; set; }


    public GenesDataResource(IDictionary<int, DataIndex> indices)
    {
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Mris = indices.Values.Any(d => d.Mris == true);
        Cts = indices.Values.Any(d => d.Cts == true);
        Tissues = indices.Values.Any(d => d.Tissues == true);
        TissuesMolecular = indices.Values.Any(d => d.TissuesMolecular == true);
        Cells = indices.Values.Any(d => d.Cells == true);
        CellsMolecular = indices.Values.Any(d => d.CellsMolecular == true);
        CellsDrugs = indices.Values.Any(d => d.CellsDrugs == true);
        Organoids = indices.Values.Any(d => d.Organoids == true);
        OrganoidsMolecular = indices.Values.Any(d => d.OrganoidsMolecular == true);
        OrganoidsDrugs = indices.Values.Any(d => d.OrganoidsDrugs == true);
        OrganoidsInterventions = indices.Values.Any(d => d.OrganoidsInterventions == true);
        Xenografts = indices.Values.Any(d => d.Xenografts == true);
        XenograftsMolecular = indices.Values.Any(d => d.XenograftsMolecular == true);
        XenograftsDrugs = indices.Values.Any(d => d.XenograftsDrugs == true);
        XenograftsInterventions = indices.Values.Any(d => d.XenograftsInterventions == true);
        Ssms = indices.Values.Any(d => d.Ssms == true);
        Cnvs = indices.Values.Any(d => d.Cnvs == true);
        Svs = indices.Values.Any(d => d.Svs == true);
        GeneExp = indices.Values.Any(d => d.GeneExp == true);
        GeneExpSc = indices.Values.Any(d => d.GeneExpSc == true);

        Total = indices.Count();
    }
}
