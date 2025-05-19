using Unite.Composer.Data.Omics.Ranges.Models.Constants;

namespace Unite.Composer.Data.Omics.Ranges.Models;

public class GenomicRangesFilterCriteria
{
    private int _startChr;
    private int _start;
    private int _endChr;
    private int _end;
    private int _density;
    private bool _male = true;

    public int StartChr { get => GetStartChr(); set => _startChr = value; }
    public int Start { get => GetStart(); set => _start = value; }
    public int EndChr { get => GetEndChr(); set => _endChr = value; }
    public int End { get => GetEnd(); set => _end = value; }
    public long Length { get => GetLength(); }
    public int Density { get => GetDensity(); set => _density = value; }
    public bool Male { get => _male; set => _male = value; }
    

    public GenomicRangesFilterCriteria()
    {
    }


    private int GetStartChr()
    {
        return _startChr > 24 ? 24
             : _startChr > 0 ? _startChr
             : 1;
    }

    private int GetStart()
    {
        var range = ChromosomeRanges.All[StartChr - 1];

        return _start > range.End ? range.End
             : _start > 0 ? _start
             : 1;
    }

    private int GetEndChr()
    {
        var lastChr = Male ? 24 : 23;

        return _endChr > lastChr ? lastChr
             : _endChr > 0 ? _endChr
             : lastChr;
    }

    private int GetEnd()
    {
        var range = ChromosomeRanges.All[EndChr - 1];

        return _end > range.End ? range.End
             : _end > 0 ? _end
             : range.End;
    }

    private long GetLength()
    {
        if (StartChr == EndChr)
        {
            return End - Start + 1;
        }
        else
        {
            long length = 0;

            for (int i = StartChr; i <= EndChr; i++)
            {
                var range = ChromosomeRanges.All[i - 1];

                length += i == StartChr ? range.End - Start + 1
                        : i == EndChr ? End
                        : range.End;
            }

            return length;
        }
    }

    private int GetDensity()
    {
        var length = GetLength();

        return _density >= length ? (int)length
             : _density >= 2048 ? 2048
             : _density >= 1024 ? 1024
             : 512;
    }
}
