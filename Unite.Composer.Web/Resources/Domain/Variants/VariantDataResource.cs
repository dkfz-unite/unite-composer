using Unite.Indices.Entities;
using Unite.Indices.Entities.Basic.Genome.Variants.Constants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class VariantDataResource
{
    public bool? Donors { get; set; }
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }
    public bool? Mris { get; set; }
    public bool? Cts { get; set; }
    public bool? Tissues { get; set; }
    public bool? TissuesMolecular { get; set; }
    public bool? Cells { get; set; }
    public bool? CellsMolecular { get; set; }
    public bool? CellsDrugs { get; set; }
    public bool? Organoids { get; set; }
    public bool? OrganoidsMolecular { get; set; }
    public bool? OrganoidsDrugs { get; set; }
    public bool? OrganoidsInterventions { get; set; }
    public bool? Xenografts { get; set; }
    public bool? XenograftsMolecular { get; set; }
    public bool? XenograftsDrugs { get; set; }
    public bool? XenograftsInterventions { get; set; }
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }


    public VariantDataResource() { }

    public VariantDataResource(DataIndex index, string type)
    {
        Donors = index.Donors;
        Clinical = index.Clinical;
        Treatments = index.Treatments;
        Mris = index.Mris;
        Cts = index.Cts;
        Tissues = index.Tissues;
        TissuesMolecular = index.TissuesMolecular;
        Cells = index.Cells;
        CellsMolecular = index.CellsMolecular;
        CellsDrugs = index.CellsDrugs;
        Organoids = index.Organoids;
        OrganoidsMolecular = index.OrganoidsMolecular;
        OrganoidsDrugs = index.OrganoidsDrugs;
        OrganoidsInterventions = index.OrganoidsInterventions;
        Xenografts = index.Xenografts;
        XenograftsMolecular = index.XenograftsMolecular;
        XenograftsDrugs = index.XenograftsDrugs;
        XenograftsInterventions = index.XenograftsInterventions;
        Ssms = type == VariantType.SSM && index.Ssms == true;
        Cnvs = type == VariantType.CNV && index.Cnvs == true;
        Svs = type == VariantType.SV && index.Svs == true;
        GeneExp = index.GeneExp;
        GeneExpSc = index.GeneExpSc;
    }
}
