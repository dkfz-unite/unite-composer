using Unite.Indices.Entities;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimensDataResource
{
    public bool Clinical { get; set; }
    public bool Treatments { get; set; }
    public bool Mris { get; set; }
    public bool Cts { get; set; }
    public bool Molecular { get; set; }
    public bool Interventions { get; set; }
    public bool Drugs { get; set; }
    public bool Ssms { get; set; }
    public bool Cnvs { get; set; }
    public bool Svs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }

    public int Total { get; set; }


    public SpecimensDataResource(IReadOnlyDictionary<object, DataIndex> indices, string type)
    {
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Mris = indices.Values.Any(d => d.Mris == true);
        Cts = indices.Values.Any(d => d.Cts == true);
        Molecular = indices.Values.Any(d => GetMolecilar(d, type) == true);
        Drugs = indices.Values.Any(d => GetDrugs(d, type) == true);
        Interventions = indices.Values.Any(d => GetInterventions(d, type) == true);
        Ssms = indices.Values.Any(d => d.Ssms == true);
        Cnvs = indices.Values.Any(d => d.Cnvs == true);
        Svs = indices.Values.Any(d => d.Svs == true);
        GeneExp = indices.Values.Any(d => d.GeneExp == true);
        GeneExpSc = indices.Values.Any(d => d.GeneExpSc == true);

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
