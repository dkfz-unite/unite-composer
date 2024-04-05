using System.Text.Json.Serialization;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public abstract class RangeData
{
    [JsonPropertyName("r")]
    public int[] Range { get; set; }

    protected RangeData()
    {

    }

    protected RangeData(int[] range)
    {
        Range = range;
    }
}

