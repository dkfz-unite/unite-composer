namespace Unite.Composer.Data.Genome.Models;

public class GenomicRange
{
    public int Chr { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public int Length => GetLength(Start, End);
    public string Code => GetCode(Chr, Start, End);


    public GenomicRange(int chr, int start, int end)
    {
        Chr = chr;
        Start = start;
        End = end;
    }


    private static int GetLength(int start, int end)
    {
        return end - start + 1;
    }

    private static string GetCode(int chr, int start, int end)
    {
        var chromosome = chr == 23 ? "X" : chr == 24 ? "Y" : $"{chr}";

        return $"{chromosome}.{start}-{end}";
    }
}
