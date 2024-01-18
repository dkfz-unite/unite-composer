using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GenesDataResource : GeneDataResource
{
    public int Total { get; set; }


    public GenesDataResource(IReadOnlyDictionary<object, DataIndex> indices)
    {
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Mris = indices.Values.Any(d => d.Mris == true);
        Cts = indices.Values.Any(d => d.Cts == true);
        Materials = indices.Values.Any(d => d.Materials == true);
        MaterialsMolecular = indices.Values.Any(d => d.MaterialsMolecular == true);
        Lines = indices.Values.Any(d => d.Lines == true);
        LinesMolecular = indices.Values.Any(d => d.LinesMolecular == true);
        LinesDrugs = indices.Values.Any(d => d.LinesDrugs == true);
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
