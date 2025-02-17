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
    public bool? Exp { get; set; }
    public bool? ExpSc { get; set; }
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? Meth { get; set; }

    public int? Total { get; set; }


    public SpecimenDataResource(DataIndex index, string type)
    {
        Donors = index.Donors;
        Clinical = index.Clinical;
        Treatments = index.Treatments;
        Molecular = GetMolecilar(index, type);
        Interventions = GetInterventions(index, type);
        Drugs = GetDrugs(index, type);
        Mris = index.Mris;
        Cts = index.Cts;
        Exp = index.Exp;
        ExpSc = index.ExpSc;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        Meth = index.Meth;
    }

    public SpecimenDataResource(IReadOnlyDictionary<object, DataIndex> indices, string type)
    {
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Molecular = indices.Values.Any(d => GetMolecilar(d, type) == true);
        Interventions = indices.Values.Any(d => GetInterventions(d, type) == true);
        Drugs = indices.Values.Any(d => GetDrugs(d, type) == true);
        Mris = indices.Values.Any(d => d.Mris == true);
        Cts = indices.Values.Any(d => d.Cts == true);
        Exp = indices.Values.Any(d => d.Exp == true);
        ExpSc = indices.Values.Any(d => d.ExpSc == true);
        Ssms = indices.Values.Any(d => d.Ssms == true);
        Cnvs = indices.Values.Any(d => d.Cnvs == true);
        Svs = indices.Values.Any(d => d.Svs == true);
        Meth = indices.Values.Any(d => d.Meth == true);

        Total = indices.Count;
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
