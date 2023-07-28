using Unite.Data.Entities.Donors;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class StudyDonorConverter : IConverter<IEnumerable<StudyDonor>>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var values = (IEnumerable<StudyDonor>)value;
        
        return string.Join(", ", values.Select(v => v.Study.Name));
    }
}
