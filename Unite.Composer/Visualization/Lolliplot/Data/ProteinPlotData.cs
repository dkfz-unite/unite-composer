namespace Unite.Composer.Visualization.Lolliplot.Data;

/// <summary>
/// Represents data required to build protein plot.
/// </summary>
public class ProteinPlotData
{
    /// <summary>
    /// Pfam domains of the protein
    /// </summary>
    public IEnumerable<ProteinDomain> Domains { get; set; }

    /// <summary>
    /// Protein mutations
    /// </summary>
    public IEnumerable<ProteinMutation> Mutations { get; set; }
}
