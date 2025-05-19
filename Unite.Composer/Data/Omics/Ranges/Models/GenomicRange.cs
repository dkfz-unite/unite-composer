using System.Text.Json.Serialization;

namespace Unite.Composer.Data.Omics.Ranges.Models;

public class GenomicRange
{
    [JsonPropertyName("i")]
    public int Index { get; set; }
    [JsonPropertyName("c")]
    public int Chr { get; set; }
    [JsonPropertyName("s")]
    public int Start { get; set; }
    [JsonPropertyName("e")]
    public int End { get; set; }

    [JsonIgnore]
    public int Length => End - Start + 1;


    public GenomicRange(int chr, int start, int end)
    {
        Chr = chr;
        Start = start;
        End = end;
    }
}
