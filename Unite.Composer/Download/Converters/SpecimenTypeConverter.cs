using Unite.Data.Entities.Specimens;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Converters;

public class SpecimenTypeConverter : IConverter<Specimen>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var specimen = (Specimen)value;

        return specimen.Tissue != null ? "Tissue"
             : specimen.CellLine != null ? "Cell Line"
             : specimen.Organoid != null ? "Organoid"
             : specimen.Xenograft != null ? "Xenograft"
             : string.Empty;
    }
}
