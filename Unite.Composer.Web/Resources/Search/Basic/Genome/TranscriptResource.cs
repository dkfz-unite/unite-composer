using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome;

public class TranscriptResource
{
    public int Id { get; set; }
    public string StableId { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public string Biotype { get; set; }
    public bool? IsCanonical { get; set; }
    public string Chromosome { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public bool? Strand { get; set; }
    public int? ExonicLength { get; set; }

    public ProteinResource Protein { get; set; }


    public TranscriptResource(TranscriptIndex index)
    {
        Id = index.Id;
        StableId = index.StableId;
        Symbol = index.Symbol;
        Description = index.Description;
        Biotype = index.Biotype;
        IsCanonical = index.IsCanonical;
        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Strand = index.Strand;
        ExonicLength = index.ExonicLength;

        if (index.Protein != null)
        {
            Protein = new ProteinResource(index.Protein);
        }
    }
}
