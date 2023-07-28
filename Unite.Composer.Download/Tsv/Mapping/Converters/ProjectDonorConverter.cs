using Unite.Data.Entities.Donors;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Tsv.Mapping.Converters;

public class ProjectDonorConverter : IConverter<IEnumerable<ProjectDonor>>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var values = (IEnumerable<ProjectDonor>)value;
        
        return string.Join(", ", values.Select(v => v.Project.Name));
    }
}
