using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Visualization.Lolliplot.Data;

public class Protein
{
    /// <summary>
    /// Unite protein id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Protein symbol
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Protein start position
    /// </summary>
    public int? Start { get; set; }

    /// <summary>
    /// Protein end position
    /// </summary>
    public int? End { get; set; }

    /// <summary>
    /// Protein length
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// Protein Ensemb id
    /// </summary>
    public string EnsemblId { get; set; }


    public Protein(ProteinIndex index)
    {
        Id = index.Id;
        Start = index.Start;
        End = index.End;
        Length = index.Length;

        EnsemblId = index.EnsemblId;
    }
}
