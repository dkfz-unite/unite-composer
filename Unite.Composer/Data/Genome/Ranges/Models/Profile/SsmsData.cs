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
    /// Variants by impact in format [High, Moderate, Low, Unknown].
    /// </summary>
    [JsonPropertyName("i")]
    public SsmImpact[] Impacts { get; set; }


    public SsmsData(int[] range, Variant variant) : base(range)
    {
        Variant = new Ssm(variant);

        Impacts = [new SsmImpact(), new SsmImpact(), new SsmImpact(), new SsmImpact()];

        SetValues(variant);
    }

    public SsmsData(int[] range, IEnumerable<Variant> variants) : base(range)
    {
        Impacts = [new SsmImpact(), new SsmImpact(), new SsmImpact(), new SsmImpact()];

        foreach (var variant in variants)
        {
            SetValues(variant);
        }
    }

    private void SetValues(Variant variant)
    {
        var consequence = variant.GetMostSeverConsequence();

        if (consequence?.Impact == "High")
        {
            Impacts[0].Total++;
            SetChangeFrom(Impacts[0], variant.Ref);
            SetChangeTo(Impacts[0], variant.Alt);
        }
        else if (consequence?.Impact == "Moderate")
        {
            Impacts[1].Total++;
            SetChangeFrom(Impacts[1], variant.Ref);
            SetChangeTo(Impacts[1], variant.Alt);
        }
        else if (consequence?.Impact == "Low")
        {
            Impacts[2].Total++;
            SetChangeFrom(Impacts[2], variant.Ref);
            SetChangeTo(Impacts[2], variant.Alt);
        }
        else if (consequence?.Impact == "Unknown")
        {
            Impacts[3].Total++;
            SetChangeFrom(Impacts[3], variant.Ref);
            SetChangeTo(Impacts[3], variant.Alt);
        }
    }

    private static void SetChangeFrom(SsmImpact impact, string nucleotide)
    {
        if (nucleotide == "A")
            impact.From[0]++;
        else if (nucleotide == "C")
            impact.From[1]++;
        else if (nucleotide == "G")
            impact.From[2]++;
        else if (nucleotide == "T")
            impact.From[3]++;
    }

    private static void SetChangeTo(SsmImpact impact, string nucleotide)
    {
        if (nucleotide == "A")
            impact.To[0]++;
        else if (nucleotide == "C")
            impact.To[1]++;
        else if (nucleotide == "G")
            impact.To[2]++;
        else if (nucleotide == "T")
            impact.To[3]++;
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
    public string Gene { get; set; }

    public Ssm(Variant variant)
    {
        var transcript = variant.GetMostAffectedTranscript();
        var consequence = variant.GetMostSeverConsequence();
        var chromosome = variant.ChromosomeId.ToDefinitionString();
        var position = variant.Start == variant.End ? $"{variant.Start}" : $"{variant.Start}-{variant.End}";

        Id = $"SSM{variant.Id}";
        Position = $"{chromosome}:{position}";
        Change = $"{variant.Ref ?? "-"}>{variant.Alt ?? "-"}";
        Type = $"{variant.TypeId}";
        Impact = consequence?.Impact;
        Consequence = consequence?.Type;
        Gene = transcript?.Feature.Symbol;
    }
}

public class SsmImpact
{
    /// <summary>
    /// Number of variants by impact in format [High, Moderate, Low, Unknown].
    /// </summary>
    [JsonPropertyName("n")]
    public int Total { get; set; } = 0;

    /// <summary>
    /// Number of variants by reference nucleotide in format [A, C, G, T].
    /// </summary>
    [JsonPropertyName("f")]
    public int[] From { get; set; } = [0, 0, 0, 0];

    /// <summary>
    /// Number of variants by alternative nucleotide in format [A, C, G, T].
    /// </summary>
    [JsonPropertyName("t")]
    public int[] To { get; set; } = [0, 0, 0, 0];
}

public static class SsmExtensions
{
    private const string HighImpact = "High";
    private const string ModerateImpact = "Moderate";
    private const string LowImpact = "Low";

    public static AffectedTranscript GetMostAffectedTranscript(this Variant variant)
    {
        return variant?.AffectedTranscripts
            .OrderBy(affectedTranscipt => affectedTranscipt.Consequences
                .Select(consequence => GetImpactGrade(consequence.Impact))
                .Min())
            .FirstOrDefault();
    }

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
