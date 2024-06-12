using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Helpers.Genome.Dna.Ssm;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

internal class CodonChangeConverter : IConverter<string>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var model = (SsmEntryWithAffectedTranscript)row;
        var change = (string)value;
        
        return CodonChangeCodeGenerator.Generate(model.AffectedFeature.CDSStart, model.AffectedFeature.CDSEnd, change);
    }
}
