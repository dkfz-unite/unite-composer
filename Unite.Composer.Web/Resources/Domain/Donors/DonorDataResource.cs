using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorDataResource
{
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }
    public bool? Mris { get; set; }
    public bool? Cts { get; set; }
    public bool? Materials { get; set; }
    public bool? MaterialsMolecular { get; set; }
    public bool? Lines { get; set; }
    public bool? LinesMolecular { get; set; }
    public bool? LinesInterventions { get; set; }
    public bool? LinesDrugs { get; set; }
    public bool? Organoids { get; set; }
    public bool? OrganoidsMolecular { get; set; }
    public bool? OrganoidsInterventions { get; set; }
    public bool? OrganoidsDrugs { get; set; }
    public bool? Xenografts { get; set; }
    public bool? XenograftsMolecular { get; set; }
    public bool? XenograftsInterventions { get; set; }
    public bool? XenograftsDrugs { get; set; }
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }


    public DonorDataResource() { }

    public DonorDataResource(DataIndex index)
    {
        Clinical = index.Clinical;
        Treatments = index.Treatments;
        Mris = index.Mris;
        Cts = index.Cts;
        Materials = index.Materials;
        MaterialsMolecular = index.MaterialsMolecular;
        Lines = index.Lines;
        LinesMolecular = index.LinesMolecular;
        LinesInterventions = index.LinesInterventions;
        LinesDrugs = index.LinesDrugs;
        Organoids = index.Organoids;
        OrganoidsMolecular = index.OrganoidsMolecular;
        OrganoidsInterventions = index.OrganoidsInterventions;
        OrganoidsDrugs = index.OrganoidsDrugs;
        Xenografts = index.Xenografts;
        XenograftsMolecular = index.XenograftsMolecular;
        XenograftsInterventions = index.XenograftsInterventions;
        XenograftsDrugs = index.XenograftsDrugs;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        GeneExp = index.GeneExp;
        GeneExpSc = index.GeneExpSc;
    }
}
