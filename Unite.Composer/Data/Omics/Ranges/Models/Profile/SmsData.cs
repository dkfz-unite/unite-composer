using System.Text.Json.Serialization;
using Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Omics.Ranges.Models.Profile;

public class SmsData : RangeData
{
    /// <summary>
    /// Variant entry.
    /// </summary>
    [JsonPropertyName("e")]
    public Sm Variant { get; set; }

    /// <summary>
    /// Variants by impact in format [High, Moderate, Low, Unknown].
    /// </summary>
    [JsonPropertyName("i")]
    public SmImpact[] Impacts { get; set; }


    public SmsData(int[] range, Variant variant) : base(range)
    {
        Variant = new Sm(variant);

        Impacts = [new SmImpact(), new SmImpact(), new SmImpact(), new SmImpact()];

        SetValues(variant);
    }

    public SmsData(int[] range, IEnumerable<Variant> variants) : base(range)
    {
        Impacts = [new SmImpact(), new SmImpact(), new SmImpact(), new SmImpact()];

        foreach (var variant in variants)
        {
            SetValues(variant);
        }
    }

    private void SetValues(Variant variant)
    {
        var effect = variant.GetMostSeverEffect();

        if (effect?.Impact == "High")
        {
            Impacts[0].Total++;
            SetChangeFrom(Impacts[0], variant.Ref);
            SetChangeTo(Impacts[0], variant.Alt);
        }
        else if (effect?.Impact == "Moderate")
        {
            Impacts[1].Total++;
            SetChangeFrom(Impacts[1], variant.Ref);
            SetChangeTo(Impacts[1], variant.Alt);
        }
        else if (effect?.Impact == "Low")
        {
            Impacts[2].Total++;
            SetChangeFrom(Impacts[2], variant.Ref);
            SetChangeTo(Impacts[2], variant.Alt);
        }
        else if (effect?.Impact == "Unknown")
        {
            Impacts[3].Total++;
            SetChangeFrom(Impacts[3], variant.Ref);
            SetChangeTo(Impacts[3], variant.Alt);
        }
    }

    private static void SetChangeFrom(SmImpact impact, string nucleotide)
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

    private static void SetChangeTo(SmImpact impact, string nucleotide)
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

public class Sm
{
    public string Id { get; set; }
    public string Position { get; set; }
    public string Type { get; set; }
    public string Change { get; set; }
    public string ChangeCodon { get; set; }
    public string ChangeProtein { get; set; }
    public string Impact { get; set; }
    public string Effect { get; set; }
    public string Gene { get; set; }

    public Sm(Variant variant)
    {
        var transcript = variant.GetMostAffectedTranscript();
        var effect = variant.GetMostSeverEffect();
        var chromosome = variant.ChromosomeId.ToDefinitionString();
        var position = variant.Start == variant.End ? $"{variant.Start}" : $"{variant.Start}-{variant.End}";

        Id = $"{variant.Id}";
        Position = $"{chromosome}:{position}";
        Change = $"{variant.Ref ?? "-"}>{variant.Alt ?? "-"}";
        ChangeCodon = transcript?.CodonChange;
        ChangeProtein = transcript?.ProteinChange;
        Type = $"{variant.TypeId}";
        Impact = effect?.Impact;
        Effect = effect?.Type;
        Gene = transcript?.Feature.Symbol;
    }
}

public class SmImpact
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

public static class SmExtensions
{
    private const string HighImpact = "High";
    private const string ModerateImpact = "Moderate";
    private const string LowImpact = "Low";

    public static AffectedTranscript GetMostAffectedTranscript(this Variant variant)
    {
        return variant?.AffectedTranscripts
            .OrderBy(affectedTranscipt => affectedTranscipt.Effects
                .Select(effect => GetImpactGrade(effect.Impact))
                .Min())
            .FirstOrDefault();
    }

    public static Unite.Data.Entities.Omics.Analysis.Dna.Effect GetMostSeverEffect(this Variant variant)
    {
        return variant?.AffectedTranscripts
            .SelectMany(affectedTranscipt => affectedTranscipt.Effects)
            .OrderBy(effect => GetImpactGrade(effect.Impact))
            .ThenBy(effect => effect.Severity)
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
