using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome;

public class GeneResource
{
    public long Id { get; }
    public string StableId { get; }
    public string Symbol { get; }
    public string Description { get; set; }
    public string Biotype { get; }
    public string Chromosome { get; }
    public int? Start { get; }
    public int? End { get; }
    public bool? Strand { get; }
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
