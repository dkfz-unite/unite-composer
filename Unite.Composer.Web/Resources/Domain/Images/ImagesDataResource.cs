using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImagesDataResource
{
    public bool Clinical { get; set; }
    public bool Treatments { get; set; }
    public bool Materials { get; set; }
    public bool MaterialsMolecular { get; set; }
    public bool Ssms { get; set; }
    public bool Cnvs { get; set; }
    public bool Svs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }

    public int Total { get; set; }


    public ImagesDataResource(IReadOnlyDictionary<object, DataIndex> indices)
    {
        Clinical = indices.Values.Any(d => d.Clinical == true);
        Treatments = indices.Values.Any(d => d.Treatments == true);
        Materials = indices.Values.Any(d => d.Materials == true);
        MaterialsMolecular = indices.Values.Any(d => d.MaterialsMolecular == true);
        Ssms = indices.Values.Any(d => d.Ssms == true);
        Cnvs = indices.Values.Any(d => d.Cnvs == true);
        Svs = indices.Values.Any(d => d.Svs == true);
        GeneExp = indices.Values.Any(d => d.GeneExp == true);
        GeneExpSc = indices.Values.Any(d => d.GeneExpSc == true);

        Total = indices.Count;
    }
}
