namespace Unite.Composer.Data.Genome.Models;

public record Protein
{
    public int Id { get; set; }
    public string StableId { get; set; }
    public string Symbol { get; set; }
    public bool IsCanonical { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Length { get; set; }


    public Protein()
    {

    }

    public Protein(Unite.Data.Entities.Genome.Protein protein)
    {
        Id = protein.Id;
        StableId = protein.StableId;
        Symbol = protein.StableId;
        IsCanonical = protein.IsCanonical.Value;
        Start = protein.Start.Value;
        End = protein.End.Value;
        Length = protein.Length.Value;
    }
}
