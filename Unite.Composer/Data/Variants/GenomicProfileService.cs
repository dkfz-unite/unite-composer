using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Models;
using Unite.Composer.Data.Variants.Models;
using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Extensions;
using Unite.Data.Services;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

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
        var ranges = _rangesService.GetRanges(filterCriteria).Select(range => new GenomicRangeData(range)).ToArray();

        var startChr = ranges.Min(range => range.Chr);
        var start = ranges.Where(range => range.Chr == startChr).Min(range => range.Start);
        var endChr = ranges.Max(range => range.Chr);
        var end = ranges.Where(range => range.Chr == endChr).Max(range => range.End);

        if (filterCriteria.Ssm)
        {
            var variants = LoadMutations(donorId, startChr, start, endChr, end);
            FillWithSsmData(in variants, ref ranges);
        }

        if (filterCriteria.Cnv) 
        {
            var variants = LoadCopyNumberVariants(donorId, startChr, start, endChr, end);
            FillWithCnvData(in variants, ref ranges);
        }

        if (filterCriteria.Exp)
        {
            var expressions = LoadGeneExpressions(donorId, startChr, start, endChr, end);
            FillWithExpressionData(in expressions, ref ranges);
        }

        var profile = new GenomicRangesData() { Ranges = ranges.ToArray() };

        return profile;
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
                range.Ssm = new SsmData();

                foreach (var variant in rangeVariants)
                {
                    var impact = variant.AffectedTranscripts?
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
            ).ToArray();

            if (rangeVariants.Any())
            {
                range.Cnv = new CnvData();

                var cnaType = rangeVariants
                    .OrderBy(variant => (int)variant.TypeId)
                    .Select(variant => variant.TypeId)
                    .FirstOrDefault();

                range.Cnv.Cna = cnaType.ToDefinitionString();
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
                range.Exp = new ExpressionData();

                foreach (var expression in rangeExpressions)
                {
                    range.Exp.Reads += expression.Reads;
                    range.Exp.TPM += expression.TPM;
                    range.Exp.FPKM += expression.FPKM;
                }

                range.Exp.TPM = Math.Round(range.Exp.TPM);
                range.Exp.TPM = Math.Round(range.Exp.TPM);
                range.Exp.FPKM = Math.Round(range.Exp.FPKM);
            }
        }
    }

    private SSM.Variant[] LoadMutations(int donorId, int startChr, int start, int endChr, int end)
    {
        return _dbContext.Set<SSM.VariantOccurrence>()
            .Include(occurrence => occurrence.Variant).ThenInclude(variant => variant.AffectedTranscripts)
            .Where(occurrence => occurrence.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .Where(occurrence => (int)occurrence.Variant.ChromosomeId >= startChr && (int)occurrence.Variant.ChromosomeId <= endChr)
            .Select(occurrence => occurrence.Variant)
            .ToArray();
    }

    private CNV.Variant[] LoadCopyNumberVariants(int donorId, int startChr, int start, int endChr, int end)
    {
        return _dbContext.Set<CNV.VariantOccurrence>()
            .Include(occurrence => occurrence.Variant).ThenInclude(variant => variant.AffectedTranscripts)
            .Where(occurrence => occurrence.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .Where(occurrence => (int)occurrence.Variant.ChromosomeId >= startChr && (int)occurrence.Variant.ChromosomeId <= endChr)
            .Select(occurrence => occurrence.Variant)
            .ToArray();
    }

    private GeneExpression[] LoadGeneExpressions(int donorId, int startChr, int start, int endChr, int end)
    {
        return _dbContext.Set<GeneExpression>()
            .Include(expression => expression.Gene)
            .Where(expression => expression.AnalysedSample.Sample.Specimen.DonorId == donorId)
            .Where(expression => (int)expression.Gene.ChromosomeId >= startChr && (int)expression.Gene.ChromosomeId <= endChr)
            .ToArray();
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
