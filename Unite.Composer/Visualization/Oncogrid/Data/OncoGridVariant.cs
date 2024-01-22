namespace Unite.Composer.Visualization.Oncogrid.Data;

public class OncoGridVariant
{
    /// <summary>
    /// Mutation id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Mutation code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Mutation most sever consequence
    /// </summary>
    public string Consequence { get; set; }

    /// <summary>
    /// Mutation most sever consequence impact
    /// </summary>
    public string Impact { get; set; }

    /// <summary>
    /// Should be equal to <see cref="OncoGridDonor.Id"/> within <see cref="OncoGridData.Donors"/>
    /// </summary>
    public string DonorId { get; set; }

    /// <summary>
    /// Should be equal to <see cref="OncoGridGene.Id"/> within <see cref="OncoGridData.Genes"/>
    /// </summary>
    public string GeneId { get; set; }
}
