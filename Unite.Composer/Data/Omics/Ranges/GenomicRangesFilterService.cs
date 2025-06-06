using Unite.Composer.Data.Omics.Ranges.Models;
using Unite.Composer.Data.Omics.Ranges.Models.Constants;

namespace Unite.Composer.Data.Omics.Ranges;

public class GenomicRangesFilterService
{
    private const int SHIFT = 1;


    public IEnumerable<GenomicRange> GetRanges(GenomicRangesFilterCriteria filterCriteria)
    {
        var criteria = filterCriteria ?? new GenomicRangesFilterCriteria();

        var chromosomeRanges = ChromosomeRanges.All
            .Where(range => range.Chr >= criteria.StartChr && range.Chr <= criteria.EndChr)
            .Select(range => new GenomicRange(range.Chr, range.Start, range.End))
            .ToArray();

        chromosomeRanges.First().Start = criteria.Start;

        chromosomeRanges.Last().End = criteria.End;

        var slice = (int)Math.Floor((double)(filterCriteria.Length / filterCriteria.Density) + SHIFT);

        foreach (var chromosomeRange in chromosomeRanges)
        {
            var coveredLength = chromosomeRange.Start;

            do
            {
                var chr = chromosomeRange.Chr;
                var start = chromosomeRange.Start == coveredLength ? chromosomeRange.Start : coveredLength + 1;
                var end = start + slice < chromosomeRange.End ? start + slice : chromosomeRange.End;
                coveredLength = end;

                yield return new GenomicRange(chr, start, end);
            }
            while (coveredLength < chromosomeRange.End);
        }
    }
}
