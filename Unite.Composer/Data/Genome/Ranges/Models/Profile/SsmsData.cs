using System.Text.Json.Serialization;
using Unite.Data.Entities.Genome.Variants.SSM;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public class SsmsData : RangeData
{
    /// <summary>
    /// Variant entry.
    /// </summary>
    [JsonPropertyName("e")]
    public Ssm Variant { get; set; }

    /// <summary>
    /// Array of variant impact stats in format [High, Moderate, Low, Unknown].
    /// </summary>
    [JsonPropertyName("i")]
    public int[] Impacts { get; set; }


    public SsmsData(int[] range, Variant variant) : base(range)
    {
        Variant = new Ssm(variant);

        Impacts = [0, 0, 0, 0];

        var consequence = variant.GetMostSeverConsequence();

        SetImpacts(consequence);
    }

    public SsmsData(int[] range, IEnumerable<Variant> variants) : base(range)
    {
        Impacts = [0, 0, 0, 0];

        foreach (var variant in variants)
        {
            var consequence = variant.GetMostSeverConsequence();

            SetImpacts(consequence);
        }
    }

    private void SetImpacts(Unite.Data.Entities.Genome.Variants.Consequence consequence)
    {
        if (consequence?.Impact == "High")
            Impacts[0]++;
        else if (consequence?.Impact == "Moderate")
            Impacts[1]++;
        else if (consequence?.Impact == "Low")
            Impacts[2]++;
        else
            Impacts[3]++;
    }
}

public class Ssm
{
    public string Id { get; set; }
    public string Position { get; set; }
    public string Type { get; set; }
    public string Change { get; set; }
    public string Impact { get; set; }
    public string Consequence { get; set; }

    public Ssm(Variant variant)
    {
        var consequence = variant.GetMostSeverConsequence();
        var chromosome = variant.ChromosomeId.ToDefinitionString();
        var position = variant.Start == variant.End ? $"{variant.Start}" : $"{variant.Start}-{variant.End}";

        Id = $"SSM{variant.Id}";
        Position = $"{chromosome}:{position}";
        Change = $"{variant.Ref ?? "-"}>{variant.Alt ?? "-"}";
        Type = $"{variant.TypeId}";
        Impact = consequence?.Impact;
        Consequence = consequence?.Type;
    }
}

public static class SsmExtensions
{
    private const string HighImpact = "High";
    private const string ModerateImpact = "Moderate";
    private const string LowImpact = "Low";

    public static Unite.Data.Entities.Genome.Variants.Consequence GetMostSeverConsequence(this Variant variant)
    {
        return variant?.AffectedTranscripts
            .SelectMany(affectedTranscipt => affectedTranscipt.Consequences)
            .OrderBy(consequence => GetImpactGrade(consequence.Impact))
            .ThenBy(consequence => consequence.Severity)
            .FirstOrDefault();
    }

    public static byte GetImpactGrade(string impact)
    {
        return impact switch
        {
            HighImpact => 1,
            ModerateImpact => 2,
            LowImpact => 3,
            _ => 4
        };
    }
}

