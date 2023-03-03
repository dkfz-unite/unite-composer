using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Visualization.Lolliplot.Data;

public class Transcript
{
    /// <summary>
    /// Unite transcript id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Protein stable id
    /// </summary>
    public string StableId { get; set; }

    /// <summary>
    /// Transcript symbol
    /// </summary>
    public string Symbol { get; set; }


    /// <summary>
    /// Protein coded by the transcript
    /// </summary>
    public Protein Protein { get; set; }


    public Transcript(TranscriptIndex index)
    {
        Id = index.Id;
        StableId = index.StableId;
        Symbol = index.Symbol;

        Protein = new Protein(index.Protein);
    }
}
