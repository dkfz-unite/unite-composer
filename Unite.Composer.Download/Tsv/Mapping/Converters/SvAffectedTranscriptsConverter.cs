using Unite.Data.Entities.Genome.Analysis.Dna.Sv;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class SvAffectedTranscriptsConverter : IConverter<IEnumerable<AffectedTranscript>>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var affectedTranscripts = (IEnumerable<AffectedTranscript>)value;

        return string.Join("; ", GetAffectedGenes(affectedTranscripts));
    }


    private IEnumerable<string> GetAffectedGenes(IEnumerable<AffectedTranscript> affectedTranscripts)
    {
        var affectedFeatureGroups = affectedTranscripts
            .OrderBy(affectedTranscript => (int)affectedTranscript.Feature.ChromosomeId)
            .ThenBy(affectedTranscript => affectedTranscript.Feature.Start)
            .Select(affectedTranscript => new {
                Gene = affectedTranscript.Feature.Gene.Symbol ?? affectedTranscript.Feature.Gene.StableId,
                Transcript = affectedTranscript.Feature.Symbol ?? affectedTranscript.Feature.StableId,
                ProteinStart = affectedTranscript.ProteinStart,
                ProteinEnd = affectedTranscript.ProteinEnd,
                Effect = affectedTranscript.Effects.OrderBy(effect => effect.Severity).First()
            })
            .GroupBy(affectedFeature => affectedFeature.Gene);

        foreach (var affectedFeatureGroup in affectedFeatureGroups)
        {
            var gene = affectedFeatureGroup.Key;

            var proteins = affectedFeatureGroup
                .Where(affectedFeature => affectedFeature.ProteinStart != null || affectedFeature.ProteinEnd != null)
                .Select(affectedFeature => GetTranscriptBreakingPoint(affectedFeature.ProteinStart, affectedFeature.ProteinEnd))
                .Distinct();

            yield return proteins.Any() ? $"{gene}({string.Join(", ", proteins)})" : $"{gene}";
        }
    }

    private string GetTranscriptBreakingPoint(int? start, int? end)
    {
        return start == end ? $"{start}" 
             : start != null && end != null ? $"{start}-{end}"
             : start != null ? $"{start}*"
             : end != null ? $"*{end}"
             : string.Empty;
    }
}
