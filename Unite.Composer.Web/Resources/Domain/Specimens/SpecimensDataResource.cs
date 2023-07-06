using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimensDataResource
{
    public bool Clinical { get; set; }
    public bool Treatments { get; set; }
    public bool Mris { get; set; }
    public bool Cts { get; set; }
    public bool Molecular { get; set; }
    public bool Drugs { get; set; }
    public bool Interventions { get; set; }
    public bool Ssms { get; set; }
    public bool Cnvs { get; set; }
    public bool Svs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }

    public int Total { get; set; }


    public SpecimensDataResource(IDictionary<int, DataIndex> indices)
    {
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Mris = indices.Values.Any(d => d.Mris == true);
        Cts = indices.Values.Any(d => d.Cts == true);
        Molecular = indices.Values.Any(d => d.Molecular == true);
        Drugs = indices.Values.Any(d => d.Drugs == true);
        Interventions = indices.Values.Any(d => d.Interventions == true);
        Ssms = indices.Values.Any(d => d.Ssms == true);
        Cnvs = indices.Values.Any(d => d.Cnvs == true);
        Svs = indices.Values.Any(d => d.Svs == true);
        GeneExp = indices.Values.Any(d => d.GeneExp == true);
        GeneExpSc = indices.Values.Any(d => d.GeneExpSc == true);

        Total = indices.Count();
    }
}
