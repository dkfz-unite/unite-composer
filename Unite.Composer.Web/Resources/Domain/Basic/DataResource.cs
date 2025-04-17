using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Basic;

public class DataResource
{
    public bool? Donors { get; set; }
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }
    public bool? Mrs { get; set; }
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
    public bool? Exp { get; set; }
    public bool? ExpSc { get; set; }
    public bool? Sms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? Meth { get; set; }

    public int? Total { get; set; }


    public DataResource(DataIndex index)
    {
        Donors = true;
        Clinical = index.Clinical;
        Treatments = index.Treatments;
        Mrs = index.Mrs;
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
        Exp = index.Exp;
        ExpSc = index.ExpSc;
        Sms = index.Sms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        Meth = index.Meth;
    }

    public DataResource(IReadOnlyDictionary<object, DataIndex> indices)
    {
        Donors = true;
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Mrs = indices.Values.Any(d => d.Mrs == true);
        Cts = indices.Values.Any(d => d.Cts == true);
        Materials = indices.Values.Any(d => d.Materials == true);
        MaterialsMolecular = indices.Values.Any(d => d.MaterialsMolecular == true);
        Lines = indices.Values.Any(d => d.Lines == true);
        LinesMolecular = indices.Values.Any(d => d.LinesMolecular == true);
        LinesInterventions = indices.Values.Any(d => d.LinesInterventions == true);
        LinesDrugs = indices.Values.Any(d => d.LinesDrugs == true);
        Organoids = indices.Values.Any(d => d.Organoids == true);
        OrganoidsMolecular = indices.Values.Any(d => d.OrganoidsMolecular == true);
        OrganoidsInterventions = indices.Values.Any(d => d.OrganoidsInterventions == true);
        OrganoidsDrugs = indices.Values.Any(d => d.OrganoidsDrugs == true);
        Xenografts = indices.Values.Any(d => d.Xenografts == true);
        XenograftsMolecular = indices.Values.Any(d => d.XenograftsMolecular == true);
        XenograftsInterventions = indices.Values.Any(d => d.XenograftsInterventions == true);
        XenograftsDrugs = indices.Values.Any(d => d.XenograftsDrugs == true);
        Exp = indices.Values.Any(d => d.Exp == true);
        ExpSc = indices.Values.Any(d => d.ExpSc == true);
        Sms = indices.Values.Any(d => d.Sms == true);
        Cnvs = indices.Values.Any(d => d.Cnvs == true);
        Svs = indices.Values.Any(d => d.Svs == true);
        Meth = indices.Values.Any(d => d.Meth == true);
        
        Total = indices.Count;
    }
}
