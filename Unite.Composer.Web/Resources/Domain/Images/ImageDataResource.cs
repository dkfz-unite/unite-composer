using Unite.Indices.Entities;

public class ImageDataResource
{
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }

    public ImageDataResource(DataIndex index)
    {
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
        GeneExp = index.GeneExp;
        GeneExpSc = index.GeneExpSc;
    }
}
