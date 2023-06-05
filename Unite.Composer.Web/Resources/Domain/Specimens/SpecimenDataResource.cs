using Unite.Indices.Entities.Specimens;

public class SpecimenDataResource
{
    public bool Molecular { get; set; }
    public bool Mris { get; set; }
    public bool Cts { get; set; }
    public bool Ssms { get; set; }
    public bool Cnvs { get; set; }
    public bool Svs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }
    public bool Drugs { get; set; }
    public bool Interventions { get; set; }


    public SpecimenDataResource(DataIndex index)
    {
        Molecular = index.Molecular;
        Mris = index.Mris;
        Cts = index.Cts;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        GeneExp = index.GeneExp;
        GeneExpSc = index.GeneExpSc;
        Drugs = index.Drugs;
        Interventions = index.Interventions;
    }
}
