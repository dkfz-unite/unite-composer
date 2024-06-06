using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Helpers.Genome.Dna.Ssm;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class ProteinChangeConverter : IConverter<string>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var model = row as SsmEntryWithAffectedTranscript;
        var change = value as string;
        
        return ProteinChangeCodeGenerator.Generate(model.AffectedFeature.ProteinStart, model.AffectedFeature.ProteinEnd, change);
    }
}
