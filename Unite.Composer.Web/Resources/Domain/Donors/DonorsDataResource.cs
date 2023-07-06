using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorsDataResource
{
    public bool Clinical { get; set; }
    public bool Treatments { get; set; }
    public bool Mris { get; set; }
    public bool Cts { get; set; }
    public bool Tissues { get; set; }
    public bool TissuesMolecular { get; set; }
    public bool Cells { get; set; }
    public bool CellsMolecular { get; set; }
    public bool CellsDrugs { get; set; }
    public bool Organoids { get; set; }
    public bool OrganoidsMolecular { get; set; }
    public bool OrganoidsDrugs { get; set; }
    public bool OrganoidsInterventions { get; set; }
    public bool Xenografts { get; set; }
    public bool XenograftsMolecular { get; set; }
    public bool XenograftsDrugs { get; set; }
    public bool XenograftsInterventions { get; set; }
    public bool Ssms { get; set; }
    public bool Cnvs { get; set; }
    public bool Svs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }

    public int Total { get; set; }


    public DonorsDataResource(IDictionary<int, DataIndex> indices)
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
