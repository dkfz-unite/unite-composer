using Unite.Data.Entities.Genome.Analysis.Dna;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class EffectsConverter : IConverter<Effect[]>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var effects = value as Effect[];
        
        var groups = effects.OrderBy(c => c.Severity).GroupBy(c => c.Impact);

        var impacts = groups.Select(g => $"{g.Key}({string.Join(", ", g.Select(c => c.Type))})");

        return string.Join("; ", impacts);
    }
}
