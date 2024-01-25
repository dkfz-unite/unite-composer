using Unite.Indices.Entities;

public class ImageDataResource
{
    public bool? Donors { get; set; }
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }
    public bool? Mris { get; set; }
    public bool? Cts { get; set; }
    public bool? Materials { get; set; }
    public bool? MaterialsMolecular { get; set; }
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }

    public ImageDataResource(DataIndex index)
    {
        Donors = index.Donors;
        Clinical = index.Clinical;
        Treatments = index.Treatments;
        Mris = index.Mris;
        Cts = index.Cts;
        Materials = index.Materials;
        MaterialsMolecular = index.MaterialsMolecular;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        GeneExp = index.GeneExp;
        GeneExpSc = index.GeneExpSc;
    }
}
