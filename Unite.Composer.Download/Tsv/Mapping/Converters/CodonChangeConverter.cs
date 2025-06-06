using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Helpers.Omics.Dna.Sm;
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
        var model = (SmEntryWithAffectedTranscript)row;
        var change = (string)value;
        
        return CodonChangeCodeGenerator.Generate(model.AffectedFeature.CDSStart, model.AffectedFeature.CDSEnd, change);
    }
}
