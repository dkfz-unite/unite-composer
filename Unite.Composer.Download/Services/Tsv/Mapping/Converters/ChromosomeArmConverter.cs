using Unite.Data.Entities.Omics.Enums;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Services.Tsv.Mapping.Converters;

public class ChromosomeArmConverter : IConverter<ChromosomeArm?>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        if (value == null)
            return string.Empty;

        var chromosomeArm = (ChromosomeArm)value;

        return chromosomeArm.ToDefinitionString();
    }
}
