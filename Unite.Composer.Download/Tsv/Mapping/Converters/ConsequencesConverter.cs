using Unite.Data.Entities.Genome.Variants;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class ConsequencesConverter : IConverter<Consequence[]>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var consequences = value as Consequence[];
        
        var groups = consequences.OrderBy(c => c.Severity).GroupBy(c => c.Impact);

        var impacts = groups.Select(g => $"{g.Key}({string.Join(", ", g.Select(c => c.Type))})");

        return string.Join("; ", impacts);
    }
}
