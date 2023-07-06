using Unite.Data.Utilities.Mutations;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Converters;

internal class CodonChangeConverter : IConverter<string>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var model = (SsmOccurrenceWithAffectedTranscript)row;
        var change = (string)value;
        
        return CodonChangeCodeGenerator.Generate(model.AffectedFeature.CDSStart, model.AffectedFeature.CDSEnd, change);
    }
}
