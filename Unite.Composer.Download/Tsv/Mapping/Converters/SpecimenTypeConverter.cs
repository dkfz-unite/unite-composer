using Unite.Data.Entities.Specimens;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class SpecimenTypeConverter : IConverter<Specimen>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var specimen = (Specimen)value;

        return specimen.Material != null ? "Material"
             : specimen.Line != null ? "Line"
             : specimen.Organoid != null ? "Organoid"
             : specimen.Xenograft != null ? "Xenograft"
             : string.Empty;
    }
}
