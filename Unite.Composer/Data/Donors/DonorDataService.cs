using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Donors.Models;
using Unite.Data.Entities.Genome.Analysis;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;

namespace Unite.Composer.Data.Donors;

public class DonorDataService
{
    private readonly DomainDbContext _dbContext;


    public DonorDataService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public IEnumerable<AnalysedSampleModel> GetAnalysedSamples(int donorId)
    {
        var specimens = LoadSpecimens(donorId);

        foreach (var specimen in specimens)
        {
            var analysedSamples = LoadAnalysedSamples(specimen.Id);

            if (analysedSamples.Any())
            {
                var analyses = analysedSamples
                    .Where(analysedSample => analysedSample.Analysis.TypeId != null)
                    .Select(analysedSample => analysedSample.Analysis.TypeId.Value)
                    .Distinct()
                    .ToArray();

                yield return new AnalysedSampleModel
                {
                    Id = specimen.Id,
                    ReferenceId = GetSpecimenReferenceId(specimen),
                    Type = GetSpecimenType(specimen),
                    Analyses = analyses
                };
            }
        }
    }


    private Specimen[] LoadSpecimens(int donorId)
    {
        var specimens = _dbContext.Set<Specimen>()
            .Include(specimen => specimen.Tissue)
            .Include(specimen => specimen.CellLine)
            .Include(specimen => specimen.Organoid)
            .Include(specimen => specimen.Xenograft)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.DonorId == donorId)
            .ToArray();

        return specimens;
    }

    private AnalysedSample[] LoadAnalysedSamples(int specimenId)
    {
        return _dbContext.Set<AnalysedSample>()
            .Include(analysedSample => analysedSample.Analysis)
            .Where(analysedSample => analysedSample.Sample.SpecimenId == specimenId)
            .Where(analysedSample => 
                analysedSample.MutationOccurrences.Count() > 0 ||
                analysedSample.CopyNumberVariantOccurrences.Count() > 0 ||
                analysedSample.StructuralVariantOccurrences.Count() > 0 ||
                analysedSample.GeneExpressions.Count() > 0)
            .ToArray();
    }

    private string GetSpecimenReferenceId(Specimen specimen)
    {
        return specimen.Tissue != null ? specimen.Tissue.ReferenceId 
             : specimen.CellLine != null ? specimen.CellLine.ReferenceId
             : specimen.Organoid != null ? specimen.Organoid.ReferenceId
             : specimen.Xenograft.ReferenceId;
    }

    private string GetSpecimenType(Specimen specimen)
    {
        return specimen.Tissue != null ? "Tissue" 
             : specimen.CellLine != null ? "CellLine"
             : specimen.Organoid != null ? "Organoid"
             : "Xenograft";
    }
}
