using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome;

public class TranscriptResource
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Biotype { get; set; }
    public string Chromosome { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public bool? Strand { get; set; }

    public string EnsemblId { get; set; }

    public ProteinResource Protein { get; set; }


    public TranscriptResource(TranscriptIndex index)
    {
        Id = index.Id;
        Symbol = index.Symbol;
        Biotype = index.Biotype;
        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Strand = index.Strand;

        EnsemblId = index.EnsemblId;

        if (index.Protein != null)
        {
            Protein = new ProteinResource(index.Protein);
        }
    }
}
