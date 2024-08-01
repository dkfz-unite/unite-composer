using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Visualization.Oncogrid.Data;

public class OncoGridGene
{
    public string Id { get; set; }
    public string Symbol { get; set; }
    public string Biotype { get; set; }
    public string Chromosome { get; set; }
    public bool? Strand { get; set; }

    public OncoGridGene(GeneIndex index)
    {
        Id = index.Id.ToString();
        Symbol = index.Symbol ?? index.StableId;
        Biotype = index.Biotype;
        Chromosome = index.Chromosome;
        Strand = index.Strand;
    }
}
