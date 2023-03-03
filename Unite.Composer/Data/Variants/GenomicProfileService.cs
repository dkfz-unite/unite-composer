using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Models;
using Unite.Composer.Data.Variants.Models;
using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Extensions;
using Unite.Data.Services;

namespace Unite.Composer.Data.Variants;

public class GenomicProfileService
{
    private readonly GenomicRangesFilterService _rangesService;
    private readonly DomainDbContext _dbContext;

    public GenomicProfileService(DomainDbContext dbContext)
    {
        _rangesService = new GenomicRangesFilterService();
        _dbContext = dbContext;
    }

    public GenomicRangesData GetProfile(int donorId, GenomicRangesFilterCriteria filterCriteria)
    {
        var ranges = _rangesService
            .GetRanges(filterCriteria)
            .Select(range => new GenomicRangeData(range))
            .ToArray();

        FillWithSsmData(donorId, ref ranges);
        FillWithCnvData(donorId, ref ranges);

        var profile = new GenomicRangesData() { Ranges = ranges.ToArray() };

        return profile;
    }

    private void FillWithSsmData(int donorId, ref GenomicRangeData[] ranges)
    {
        var startChr = ranges.Min(range => range.Chr);
        var start = ranges.Where(range => range.Chr == startChr).Min(range => range.Start);
        var endChr = ranges.Max(range => range.Chr);
        var end = ranges.Where(range => range.Chr == endChr).Max(range => range.End);

        var variants = _dbContext.Set<Unite.Data.Entities.Genome.Variants.SSM.VariantOccurrence>()
            .Include(variant => variant.Variant).ThenInclude(ssm => ssm.AffectedTranscripts)
            .Where(variant => variant.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .Where(variant => (int)variant.Variant.ChromosomeId >= startChr && (int)variant.Variant.ChromosomeId <= endChr)
            .ToArray();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.Variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.Variant.End >= range.Start && variant.Variant.End <= range.End) ||
                (variant.Variant.Start >= range.Start && variant.Variant.Start <= range.End) ||
                (variant.Variant.Start >= range.Start && variant.Variant.End <= range.End) ||
                (variant.Variant.Start <= range.Start && variant.Variant.End >= range.End))
            );

            if (rangeVariants.Any())
            {
                range.Ssm = new SsmData();

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

    private void FillWithCnvData(int donorId, ref GenomicRangeData[] ranges)
    {
        var startChr = ranges.Min(range => range.Chr);
        var start = ranges.Where(range => range.Chr == startChr).Min(range => range.Start);
        var endChr = ranges.Max(range => range.Chr);
        var end = ranges.Where(range => range.Chr == endChr).Max(range => range.End);

        var variants = _dbContext.Set<Unite.Data.Entities.Genome.Variants.CNV.VariantOccurrence>()
            .Include(variant => variant.Variant)
            .Where(variant => variant.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .Where(variant => (int)variant.Variant.ChromosomeId >= startChr && (int)variant.Variant.ChromosomeId <= endChr)
            .ToArray();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.Variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.Variant.End >= range.Start && variant.Variant.End <= range.End) ||
                (variant.Variant.Start >= range.Start && variant.Variant.Start <= range.End) ||
                (variant.Variant.Start >= range.Start && variant.Variant.End <= range.End) ||
                (variant.Variant.Start <= range.Start && variant.Variant.End >= range.End))
            );

            if (rangeVariants.Any())
            {
                range.Cnv = new CnvData();

                var cnaType = rangeVariants
                    .OrderBy(variant => (int)variant.Variant.TypeId)
                    .Select(variant => variant.Variant.TypeId)
                    .FirstOrDefault();

                range.Cnv.Cna = cnaType.ToDefinitionString();
            }
        }
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
