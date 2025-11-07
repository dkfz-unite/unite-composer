using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Services.Tsv.Mapping.Converters;

public class ArrayConverter<T> : IConverter<IEnumerable<T>>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var values = (IEnumerable<T>)value;
        
        return string.Join(", ", values);
    }
}
