using System.Text.Json.Serialization;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Omics.Ranges.Models.Profile;

public class ProteinsData : RangeData
{
    /// <summary>
    /// Protein entry.
    /// </summary>
    [JsonPropertyName("e")]
    public Protein Protein { get; set; }

    public ProteinsData(int[] range, Unite.Data.Entities.Omics.Protein protein) : base(range)
    {
        Protein = new Protein(protein);
    }
}

public class Protein
{
    public string Id { get; set; }
    public string Position { get; set; }
    public string Symbol { get; set; }
    public int Length { get; set; }

    public Protein(Unite.Data.Entities.Omics.Protein protein)
    {
        Id = $"{protein.Id}";
        Position = $"{protein.ChromosomeId.ToDefinitionString()}:{protein.Start}-{protein.End}";
        Symbol = protein.Symbol;
        Length = protein.Length.Value;
    }
}
