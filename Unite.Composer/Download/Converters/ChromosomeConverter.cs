using Unite.Data.Entities.Genome.Enums;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Composer.Download.Converters;

public class ChromosomeConverter : IConverter<Chromosome>
{
    public object Convert(string value, string row)
    {
        throw new NotImplementedException();
    }

    public string Convert(object value, object row)
    {
        var chromosome = (Chromosome)value;

        return (int)chromosome <= 22 ? ((int)chromosome).ToString() :
               (int)chromosome == 23 ? "X" :
               (int)chromosome == 24 ? "Y" :
               string.Empty;
    }
}
