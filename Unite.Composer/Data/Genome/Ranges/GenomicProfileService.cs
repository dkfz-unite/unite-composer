using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Services;

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

    public async Task<GenomicRangesData> GetProfile(int sampleId, GenomicRangesFilterCriteria filterCriteria)
    {
        var ranges = _rangesService.GetRanges(filterCriteria).Select(range => new GenomicRangeData(range)).ToArray();

        var startChr = ranges.Min(range => range.Chr);
        var start = ranges.Where(range => range.Chr == startChr).Min(range => range.Start);
        var endChr = ranges.Max(range => range.Chr);
        var end = ranges.Where(range => range.Chr == endChr).Max(range => range.End);

        var ssmsTask = LoadMutations(sampleId, startChr, start, endChr, end);
        var cnvsTask = LoadCopyNumberVariants(sampleId, startChr, start, endChr, end);
        var expressionsTask = LoadGeneExpressions(sampleId, startChr, start, endChr, end);

        await Task.WhenAll(ssmsTask, cnvsTask, expressionsTask);

        var ssms = ssmsTask.Result;
        var cnvs = cnvsTask.Result;
        var expressions = expressionsTask.Result;

        FillWithSsmData(ssms, ref ranges);
        FillWithCnvData(cnvs, ref ranges);
        FillWithExpressionData(expressions, ref ranges);

        return new GenomicRangesData(ranges.ToArray());
    }

    private void FillWithSsmData(in SSM.Variant[] variants, ref GenomicRangeData[] ranges)
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

    private void FillWithCnvData(in CNV.Variant[] variants, ref GenomicRangeData[] ranges)
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

    private void FillWithExpressionData(in GeneExpression[] expressions, ref GenomicRangeData[] ranges)
    {
        foreach (var range in ranges)
        {
            var rangeExpressions = expressions.Where(expression =>
                expression.Gene.ChromosomeId == (Chromosome)range.Chr &&
                ((expression.Gene.End >= range.Start && expression.Gene.End <= range.End) ||
                (expression.Gene.Start >= range.Start && expression.Gene.Start <= range.End) ||
                (expression.Gene.Start >= range.Start && expression.Gene.End <= range.End) ||
                (expression.Gene.Start <= range.Start && expression.Gene.End >= range.End))
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

    private async Task<SSM.Variant[]> LoadMutations(int sampleId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<SSM.VariantOccurrence>().AsNoTracking()
            .Include(occurrence => occurrence.Variant).ThenInclude(variant => variant.AffectedTranscripts)
            .Where(occurrence => occurrence.AnalysedSample.SampleId == sampleId)
            .Where(occurrence => (int)occurrence.Variant.ChromosomeId >= startChr && (int)occurrence.Variant.ChromosomeId <= endChr)
            .Select(occurrence => occurrence.Variant)
            .ToArrayAsync();
    }

    private async Task<CNV.Variant[]> LoadCopyNumberVariants(int sampleId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<CNV.VariantOccurrence>()
            .AsNoTracking()
            .Include(occurrence => occurrence.Variant).ThenInclude(variant => variant.AffectedTranscripts)
            .Where(occurrence => occurrence.AnalysedSample.SampleId == sampleId)
            .Where(occurrence => (int)occurrence.Variant.ChromosomeId >= startChr && (int)occurrence.Variant.ChromosomeId <= endChr)
            .Select(occurrence => occurrence.Variant)
            .ToArrayAsync();
    }

    private async Task<GeneExpression[]> LoadGeneExpressions(int sampleId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(expression => expression.Gene)
            .Where(expression => expression.AnalysedSample.SampleId == sampleId)
            .Where(expression => (int)expression.Gene.ChromosomeId >= startChr && (int)expression.Gene.ChromosomeId <= endChr)
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
