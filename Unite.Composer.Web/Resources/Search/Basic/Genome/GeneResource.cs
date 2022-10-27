using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome;

public class GeneResource
{
    public long Id { get; }
    public string Symbol { get; }
    public string Biotype { get; }
    public string Chromosome { get; }
    public int? Start { get; }
    public int? End { get; }
    public bool? Strand { get; }

    public string EnsemblId { get; }


    public GeneResource(GeneIndex index)
    {
        Id = index.Id;
        Symbol = index.Symbol;
        Biotype = index.Biotype;
        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Strand = index.Strand;

        EnsemblId = index.EnsemblId;
    }
}
