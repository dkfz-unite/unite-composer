namespace Unite.Composer.Visualization.Lolliplot.Data;

/// <summary>
/// Represents protein mutation data.
/// </summary>
public class ProteinMutation
{
    /// <summary>
    /// Unite mutation id 
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Mutation code in HGVs format
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Mutation effect code
    /// </summary>
    public string Effect { get; set; }

    /// <summary>
    /// Mutation impact
    /// </summary>
    public string Impact { get; set; }

    /// <summary>
    /// Amino acid change in protein affected by the mutation
    /// </summary>
    public string ProteinChange { get; set; }

    /// <summary>
    /// Number of donors affected by the mutation
    /// </summary>
    public int NumberOfDonors { get; set; }
}
