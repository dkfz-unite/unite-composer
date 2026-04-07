using Unite.Indices.Entities.Basic.Omics;

namespace Unite.Composer.Web.Resources.Domain.Basic.Omics;

public class GeneResource : GeneResourceBase
{
    public string Description { get; set; }
    public string Biotype { get; set; }
    public string Chromosome { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public bool? Strand { get; set; }
    public int? ExonicLength { get; set; }
    

    public GeneResource(GeneIndex index)
    {
        Id = index.Id;
        StableId = index.StableId;
        Symbol = index.Symbol;
        Description = index.Description;
        Biotype = index.Biotype;
        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Strand = index.Strand;
        ExonicLength = index.ExonicLength;
    }
}
