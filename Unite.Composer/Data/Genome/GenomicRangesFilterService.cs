using Unite.Composer.Data.Genome.Models;
using Unite.Composer.Data.Genome.Models.Constants;

namespace Unite.Composer.Data.Genome;

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

        long totalLength = chromosomeRanges.Sum(range => (long)range.End - (long)range.Start + 1);

        foreach (var chromosomeRange in chromosomeRanges)
        {
            var percent = Math.Round((double)(chromosomeRange.Length * 100.0000 / totalLength), 4);
            var parts = (int)Math.Round((double)(criteria.Density / 100.0000 * percent), 0);
            var slice = (int)Math.Round((double)(chromosomeRange.Length / parts + SHIFT), 0);

            var coveredLength = chromosomeRange.Start;

            do
            {
                var chr = chromosomeRange.Chr;
                var start = chromosomeRange.Start == coveredLength ? chromosomeRange.Start : coveredLength + 1;
                var end = start + slice < chromosomeRange.End ? start + slice : chromosomeRange.End;
                coveredLength = end;
                //coveredLength += slice;
                //var end = coveredLength < chromosomeRange.End ? coveredLength : chromosomeRange.End;

                yield return new GenomicRange(chr, start, end);
            }
            while (coveredLength < chromosomeRange.End);
        }
    }
}
