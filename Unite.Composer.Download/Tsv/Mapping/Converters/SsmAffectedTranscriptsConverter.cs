using Unite.Data.Entities.Genome.Analysis.Dna.Ssm;
using Unite.Data.Helpers.Genome.Dna.Ssm;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class SsmAffectedTranscriptsConverter: IConverter<IEnumerable<AffectedTranscript>>
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
                ProteinChange = affectedTranscript.AminoAcidChange,
                Effect = affectedTranscript.Effects.OrderBy(effect => effect.Severity).First()
            })
            .GroupBy(affectedFeature => affectedFeature.Gene);

        foreach (var affectedFeatureGroup in affectedFeatureGroups)
        {
            var gene = affectedFeatureGroup.Key;

            var proteins = affectedFeatureGroup
                .Where(affectedFeature => affectedFeature.ProteinChange != null)
                .Select(affectedFeature => ProteinChangeCodeGenerator.Generate(affectedFeature.ProteinStart, affectedFeature.ProteinEnd, affectedFeature.ProteinChange))
                .Distinct();

            yield return proteins.Any() ? $"{gene}({string.Join(", ", proteins)})" : $"{gene}";
        }
    }
}
