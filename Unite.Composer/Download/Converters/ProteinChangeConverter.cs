using Unite.Data.Utilities.Mutations;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Converters;

public class ProteinChangeConverter : IConverter<string>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var model = row as SsmOccurrenceWithAffectedTranscript;
        var change = value as string;
        
        return AAChangeCodeGenerator.Generate(model.AffectedFeature.ProteinStart, model.AffectedFeature.ProteinEnd, change);
    }
}
