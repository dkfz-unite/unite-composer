namespace Unite.Composer.Visualization.Lolliplot.Data;

public record Transcript
{
    public int Id { get; set; }
    public string StableId { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public string Biotype { get; set; }
    public bool IsCanonical { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public bool Strand { get; set; }

    public Protein Protein { get; set; }

    public Transcript() 
    {

    }

    public Transcript(Unite.Data.Entities.Genome.Transcript transcript)
    {
        Id = transcript.Id;
        StableId = transcript.StableId;
        Symbol = transcript.Symbol;
        Description = transcript.Description;
        Biotype = transcript.Biotype;
        IsCanonical = transcript.IsCanonical.Value;
        Start = transcript.Start.Value;
        End = transcript.End.Value;
        Strand = transcript.Strand.Value;

        if (transcript.Protein != null)
        {
            Protein = new Protein(transcript.Protein);
        }
    }
}
