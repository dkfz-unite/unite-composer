using Unite.Indices.Entities;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenDataResource
{
    public bool? Donors { get; set; }
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }
    public bool? Molecular { get; set; }
    public bool? Interventions { get; set; }
    public bool? Drugs { get; set; }
    public bool? Mris { get; set; }
    public bool? Cts { get; set; }
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }


    public SpecimenDataResource(DataIndex index, string type)
    {
        Donors = index.Donors;
        Clinical = index.Clinical;
        Treatments = index.Treatments;
        Mris = index.Mris;
        Cts = index.Cts;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        GeneExp = index.GeneExp;
        GeneExpSc = index.GeneExpSc;
        Molecular = GetMolecilar(index, type);
        Interventions = GetInterventions(index, type);
        Drugs = GetDrugs(index, type);
    }

    private static bool? GetMolecilar(DataIndex index, string type)
    {
        return type switch
        {
            SpecimenType.Material => index.MaterialsMolecular,
            SpecimenType.Line => index.LinesMolecular,
            SpecimenType.Xenograft => index.XenograftsMolecular,
            SpecimenType.Organoid => index.OrganoidsMolecular,
            _ => null
        };
    }

    private static bool? GetInterventions(DataIndex index, string type)
    {
        return type switch
        {
            SpecimenType.Line => index.LinesInterventions,
            SpecimenType.Xenograft => index.XenograftsInterventions,
            SpecimenType.Organoid => index.OrganoidsInterventions,
            _ => null
        };
    }

    private static bool? GetDrugs(DataIndex index, string type)
    {
        return type switch
        {
            SpecimenType.Line => index.LinesDrugs,
            SpecimenType.Xenograft => index.XenograftsDrugs,
            SpecimenType.Organoid => index.OrganoidsDrugs,
            _ => null
        };
    }
}
