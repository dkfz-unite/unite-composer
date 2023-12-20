using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Entities.Genome.Transcriptomics;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Data.Genome.Ranges;

public class GenomicProfileService
{
    private readonly GenomicRangesFilterService _rangesService;
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;

    public GenomicProfileService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _rangesService = new GenomicRangesFilterService();
        _dbContextFactory = dbContextFactory;
    }

    public async Task<GenomicRangesData> GetProfile(int specimenId, GenomicRangesFilterCriteria filterCriteria)
    {
        var ranges = _rangesService.GetRanges(filterCriteria).Select(range => new GenomicRangeData(range)).ToArray();

        var startChr = ranges.Min(range => range.Chr);
        var start = ranges.Where(range => range.Chr == startChr).Min(range => range.Start);
        var endChr = ranges.Max(range => range.Chr);
        var end = ranges.Where(range => range.Chr == endChr).Max(range => range.End);

        var ssmsTask = LoadSsms(specimenId, startChr, start, endChr, end);
        var cnvsTask = LoadCnvs(specimenId, startChr, start, endChr, end);
        var expressionsTask = LoadBulkExpressions(specimenId, startChr, start, endChr, end);

        await Task.WhenAll(ssmsTask, cnvsTask, expressionsTask);

        var ssms = ssmsTask.Result;
        var cnvs = cnvsTask.Result;
        var expressions = expressionsTask.Result;

        FillWithSsmData(ssms, ref ranges);
        FillWithCnvData(cnvs, ref ranges);
        FillWithExpressionData(expressions, ref ranges);

        return new GenomicRangesData(ranges.ToArray());
    }


    private static void FillWithSsmData(in SSM.Variant[] variants, ref GenomicRangeData[] ranges)
    {
        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.End >= range.Start && variant.End <= range.End) ||
                (variant.Start >= range.Start && variant.Start <= range.End) ||
                (variant.Start >= range.Start && variant.End <= range.End) ||
                (variant.Start <= range.Start && variant.End >= range.End))
            ).ToArray();

            if (rangeVariants.Any())
            {
                range.Ssm = new Models.Profile.MutationsData();

                foreach (var variant in rangeVariants)
                {
                    var impact = variant.AffectedTranscripts?
                        .SelectMany(affectedTranscript => affectedTranscript.Consequences)
                        .Select(consequence => GetImpactGrade(consequence.Impact))
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

    private static void FillWithCnvData(in CNV.Variant[] variants, ref GenomicRangeData[] ranges)
    {
        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.End >= range.Start && variant.End <= range.End) ||
                (variant.Start >= range.Start && variant.Start <= range.End) ||
                (variant.Start >= range.Start && variant.End <= range.End) ||
                (variant.Start <= range.Start && variant.End >= range.End))
            );

            var variant = rangeVariants.FirstOrDefault();

            if (variant != null)
            {
                range.Cnv = new Models.Profile.CopyNumberVariantsData
                {
                    Cna = variant.TypeId,
                    Loh = variant.Loh,
                    Del = variant.HomoDel,
                    Tcn = variant.TcnMean != null ? Math.Round(variant.TcnMean.Value, 2) : null
                };
            }
        }
    }

    private static void FillWithExpressionData(in BulkExpression[] expressions, ref GenomicRangeData[] ranges)
    {
        foreach (var range in ranges)
        {
            var rangeExpressions = expressions.Where(expression =>
                expression.Entity.ChromosomeId == (Chromosome)range.Chr &&
                ((expression.Entity.End >= range.Start && expression.Entity.End <= range.End) ||
                (expression.Entity.Start >= range.Start && expression.Entity.Start <= range.End) ||
                (expression.Entity.Start >= range.Start && expression.Entity.End <= range.End) ||
                (expression.Entity.Start <= range.Start && expression.Entity.End >= range.End))
            ).ToArray();

            if (rangeExpressions.Any())
            {
                range.Exp = new Models.Profile.ExpressionsData
                {
                    Reads = rangeExpressions.Average(expression => expression.Reads),
                    TPM = Math.Round(rangeExpressions.Average(expression => expression.TPM), 2),
                    FPKM = Math.Round(rangeExpressions.Average(expression => expression.FPKM), 2)
                };
            }
        }
    }


    private async Task<SSM.Variant[]> LoadSsms(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<SSM.VariantEntry>().AsNoTracking()
            .Include(entry => entry.Entity.AffectedTranscripts)
            .Where(entry => entry.AnalysedSample.TargetSampleId == specimenId)
            .Where(entry => (int)entry.Entity.ChromosomeId >= startChr && (int)entry.Entity.ChromosomeId <= endChr)
            .Select(entry => entry.Entity)
            .ToArrayAsync();
    }

    private async Task<CNV.Variant[]> LoadCnvs(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<CNV.VariantEntry>()
            .AsNoTracking()
            .Include(entry => entry.Entity.AffectedTranscripts)
            .Where(entry => entry.AnalysedSample.TargetSampleId == specimenId)
            .Where(entry => (int)entry.Entity.ChromosomeId >= startChr && (int)entry.Entity.ChromosomeId <= endChr)
            .Select(entry => entry.Entity)
            .ToArrayAsync();
    }

    private async Task<BulkExpression[]> LoadBulkExpressions(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<BulkExpression>()
            .AsNoTracking()
            .Include(expression => expression.Entity)
            .Where(expression => expression.AnalysedSample.TargetSampleId == specimenId)
            .Where(expression => (int)expression.Entity.ChromosomeId >= startChr && (int)expression.Entity.ChromosomeId <= endChr)
            .ToArrayAsync();
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
