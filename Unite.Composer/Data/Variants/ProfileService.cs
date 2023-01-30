using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Services;

namespace Unite.Composer.Data.Variants;

public class GenomicRange
{
    public int Chr { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public double Tcn { get; set; }

    public GenomicRangeStats Ssm { get; set; }
    public GenomicRangeStats Sv { get; set; }

    public int Length => End - Start;
    public string Label => $"{Chr}.{Start} - {End}";

    public GenomicRange(int chr, int start, int end)
    {
        Chr = chr;
        Start = start;
        End = end;

        Tcn = 2;
    } 
}

public class GenomicRangeStats
{
    [JsonPropertyName("h")]
    public int High { get; set; }
    [JsonPropertyName("m")]
    public int Moderate { get; set; }
    [JsonPropertyName("l")]
    public int Low { get; set; }
    [JsonPropertyName("u")]
    public int Unknown { get; set; }
}

public class ProfileService
{
    private static readonly Dictionary<Chromosome, int> _chromosomes = new Dictionary<Chromosome, int>()
    {
        { Chromosome.Chr1, 249250621 },
        { Chromosome.Chr2, 243199373 },
        { Chromosome.Chr3, 198022430 },
        { Chromosome.Chr4, 191154276 },
        { Chromosome.Chr5, 180915260 },
        { Chromosome.Chr6, 171115067 },
        { Chromosome.Chr7, 159138663 },
        { Chromosome.Chr8, 146364022 },
        { Chromosome.Chr9, 141213431 },
        { Chromosome.Chr10, 135534747 },
        { Chromosome.Chr11, 135006516 },
        { Chromosome.Chr12, 133851895 },
        { Chromosome.Chr13, 115169878 },
        { Chromosome.Chr14, 107349540 },
        { Chromosome.Chr15, 102531392 },
        { Chromosome.Chr16, 90354753 },
        { Chromosome.Chr17, 81195210 },
        { Chromosome.Chr18, 78077248 },
        { Chromosome.Chr19, 59128983 },
        { Chromosome.Chr20, 63025520 },
        { Chromosome.Chr21, 48129895 },
        { Chromosome.Chr22, 51304566 },
        { Chromosome.ChrX, 155270560 },
        { Chromosome.ChrY, 59373566 }
    };

    private static readonly (int Chr, int Start, int End)[] _ranges = new (int Chr, int Start, int End)[]
    {
        ( 1, 0, 249250621 ),
        ( 2, 0, 243199373 ),
        ( 3, 0, 198022430 ),
        ( 4, 0, 191154276 ),
        ( 5, 0, 180915260 ),
        ( 6, 0, 171115067 ),
        ( 7, 0, 159138663 ),
        ( 8, 0, 146364022 ),
        ( 9, 0, 141213431 ),
        ( 10, 0, 135534747 ),
        ( 11, 0, 135006516 ),
        ( 12, 0, 133851895 ),
        ( 13, 0, 115169878 ),
        ( 14, 0, 107349540 ),
        ( 15, 0, 102531392 ),
        ( 16, 0, 90354753 ),
        ( 17, 0, 81195210 ),
        ( 18, 0, 78077248 ),
        ( 19, 0, 59128983 ),
        ( 20, 0, 63025520 ),
        ( 21, 0, 48129895 ),
        ( 22, 0, 51304566 ),
        ( 23, 0, 155270560 ), // X
        ( 24, 0, 59373566 ) // Y
    };

    private static readonly Random _random = new Random();

    private readonly DomainDbContext _dbContext;

    public ProfileService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GenomicRange[] GetProfile(int donorId)
    {
        var ranges = GetRanges();

        FillWithMutations(donorId, ref ranges);
        FillWithCopyNumberVariants(donorId, ref ranges);
        //FillWithStructuralVariants(donorId, ref ranges);

        return ranges;
    }

    private void FillWithMutations(int donorId, ref GenomicRange[] ranges)
    {
        var variants = _dbContext.Set<Unite.Data.Entities.Genome.Variants.SSM.VariantOccurrence>()
            .Include(variant => variant.Variant).ThenInclude(ssm => ssm.AffectedTranscripts)
            .Where(variant => variant.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .ToArray();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.Variant.ChromosomeId == (Chromosome)range.Chr &&
                variant.Variant.Start >= range.Start &&
                variant.Variant.End <= range.End
            );

            if (rangeVariants.Any())
            {
                range.Ssm = new GenomicRangeStats();

                foreach (var variant in rangeVariants)
                {
                    var impact = variant.Variant.AffectedTranscripts?
                        .SelectMany(affectedTranscript => affectedTranscript.Consequences)
                        .Select(consequence => GetImpactGrade(consequence.Impact))
                        .Distinct()
                        .OrderBy(grade => grade)
                        .FirstOrDefault();

                    if (impact == 1)
                        range.Ssm.High++;
                    else if (impact == 2)
                        range.Ssm.Moderate++;
                    else if (impact == 3)
                        range.Ssm.Low++;
                    else
                        range.Ssm.Unknown++;
                }
            }
        }
    }

    private void FillWithCopyNumberVariants(int donorId, ref GenomicRange[] ranges)
    {
        var variants = _dbContext.Set<Unite.Data.Entities.Genome.Variants.CNV.VariantOccurrence>()
            .Include(variant => variant.Variant)
            .Where(variant => variant.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .ToArray();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.Variant.ChromosomeId == (Chromosome)range.Chr &&
                variant.Variant.Start >= range.Start &&
                variant.Variant.End <= range.End
            );

            if (rangeVariants.Any())
            {
                var withTcnMean = rangeVariants
                    .FirstOrDefault(variant => variant.Variant.TcnMean >= 0);

                if(withTcnMean != null)
                {
                    range.Tcn = withTcnMean.Variant.TcnMean.Value;
                    continue;
                }

                var withTcn = rangeVariants
                    .FirstOrDefault(variant => variant.Variant.Tcn >= 0);

                if (withTcn != null)
                {
                    range.Tcn = withTcn.Variant.Tcn.Value;
                    continue;
                }

                var cnaType = rangeVariants
                    .OrderBy(variant => (int)variant.Variant.CnaTypeId)
                    .Select(variant => variant.Variant.CnaTypeId)
                    .FirstOrDefault();

                if (cnaType == Unite.Data.Entities.Genome.Variants.CNV.Enums.CnaType.Gain)
                    range.Tcn = 4;
                else if (cnaType == Unite.Data.Entities.Genome.Variants.CNV.Enums.CnaType.Loss)
                    range.Tcn = 0;
                else
                    range.Tcn = 2;
            }
        }
    }

    private void FillWithStructuralVariants(int donorId, ref GenomicRange[] ranges)
    {
        var variants = _dbContext.Set<Unite.Data.Entities.Genome.Variants.SV.VariantOccurrence>()
            .Include(variant => variant.Variant)
                .ThenInclude(sv => sv.AffectedTranscripts)
                    .ThenInclude(affectedTranscript => affectedTranscript.Feature)
            .Where(variant => variant.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .ToArray();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.Variant.ChromosomeId == (Chromosome)range.Chr &&
                variant.Variant.Start >= range.Start &&
                variant.Variant.OtherChromosomeId == (Chromosome)range.Chr &&
                variant.Variant.OtherEnd <= range.End
            );

            if (rangeVariants.Any())
            {
                range.Sv = new GenomicRangeStats();

                foreach (var variant in rangeVariants)
                {
                    var impact = variant.Variant.AffectedTranscripts?
                        .Where(affectedTranscript => affectedTranscript.Feature.Start >= range.Start && affectedTranscript.Feature.End < range.End)
                        .SelectMany(affectedTranscript => affectedTranscript.Consequences)
                        .Select(consequence => GetImpactGrade(consequence.Impact))
                        .Distinct()
                        .OrderBy(grade => grade)
                        .FirstOrDefault();

                    if (impact == 1)
                        range.Sv.High++;
                    else if (impact == 2)
                        range.Sv.Moderate++;
                    else if (impact == 3)
                        range.Sv.Low++;
                    else
                        range.Sv.Unknown++;
                }
            }
        }
    }

    private static GenomicRange[] GetRanges(int startChr = 1, int startChrStart = 0, int endChr = 24, int endChrEnd = 59373566, int density = 1024 + 512, int shift = 1)
    {
        var chromosomeRanges = _ranges
            .Where(range => range.Chr >= startChr && range.Chr <= endChr)
            .Select(range => new GenomicRange(range.Chr, range.Start, range.End))
            .ToArray();

        chromosomeRanges.First().Start = startChrStart;
        chromosomeRanges.Last().End = endChrEnd;

        long totalLength = chromosomeRanges.Sum(range => (long)range.Length);

        var genomicRanges = chromosomeRanges.SelectMany(range =>
        {
            var percent = Math.Round((double)(range.Length * 100.0000 / totalLength), 4);
            var parts = (int)Math.Round((double)(density / 100.0000 * percent), 0);
            var slice = (int)Math.Round((double)(range.Length / parts + shift), 0);

            var ranges = new List<GenomicRange>();
            var total = 0;

            do
            {
                var chr = range.Chr;
                var start = total + 1;
                total += slice;
                var end = total < range.Length ? total : range.Length;

                var values = new GenomicRange(chr, start, end);

                ranges.Add(values);
            }
            while (total < range.Length);

            return ranges;
        });

        return genomicRanges.ToArray();

    }

    private static int GetImpactGrade(string impactType)
    {
        return impactType switch
        {
            "High" => 1,
            "Moderate" => 2,
            "Low" => 3,
            "Unknown" => 4,
            _ => 5
        };
    }
}
