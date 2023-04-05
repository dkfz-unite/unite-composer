using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Projects.Models;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;

using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Data.Projects;

public class ProjectService
{
    private readonly DomainDbContext _dbContext;


    public ProjectService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ProjectModel Get(int id)
    {
        var project = _dbContext.Set<Project>()
            .FirstOrDefault(project => project.Id == id);

        if (project != null)
        {
            var projectDonorIds = _dbContext.Set<ProjectDonor>()
                .Where(projectDonor => projectDonor.ProjectId == project.Id)
                .Select(projectDonor => projectDonor.DonorId)
                .Distinct()
                .ToArray();

            var projectModel = new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Data = CalculateProjectData(projectDonorIds)
            };

            return projectModel;
        }
        else
        {
            return null;
        }
    }

    public ProjectModel Update(ProjectModel projectModel)
    {
        var project = _dbContext.Set<Project>()
            .FirstOrDefault(project => project.Id == projectModel.Id);

        if (project != null)
        {
            project.Description = projectModel.Description;

            _dbContext.Update(project);
            _dbContext.SaveChanges();

            return Get(projectModel.Id.Value);
        }
        else
        {
            return null;
        }
    }

    public IEnumerable<ProjectModel> GetAll()
    {
        var projects = _dbContext.Set<Project>()
            .OrderBy(project => project.Name)
            .ToArray();

        foreach (var project in projects)
        {
            var projectDonorIds = _dbContext.Set<ProjectDonor>()
                .Where(projectDonor => projectDonor.ProjectId == project.Id)
                .Select(projectDonor => projectDonor.DonorId)
                .Distinct()
                .ToArray();

            var projectModel = new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Data = CalculateProjectData(projectDonorIds)
            };


            yield return projectModel;
        }

        var emptyProjectDonorIds = _dbContext.Set<Donor>()
            .Where(donor => donor.DonorProjects.Count == 0)
            .Select(donor => donor.Id)
            .Distinct()
            .ToArray();

        if (emptyProjectDonorIds.Length > 0)
        {
            var emptyProjectModel = new ProjectModel
            {
                Id = 0,
                Name = "Other",
                Description = "Data not assigned to any project",
                Data = CalculateProjectData(emptyProjectDonorIds)
            };

            yield return emptyProjectModel;
        }
    }


    private ProjectDataModel CalculateProjectData(int[] donorIds)
    {
        var withMriImages = _dbContext.Set<Image>()
            .Include(image => image.MriImage)
            .Where(image => image.MriImage != null)
            .Where(image => donorIds.Contains(image.DonorId))
            .Select(image => image.DonorId)
            .Distinct()
            .Count();

        var withCtImages = 0;

        var withTissues = _dbContext.Set<Specimen>()
            .Include(specimen => specimen.Tissue)
            .Where(specimen => specimen.Tissue != null)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withCellLines = _dbContext.Set<Specimen>()
            .Include(specimen => specimen.CellLine)
            .Where(specimen => specimen.CellLine != null)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withOrganoids = _dbContext.Set<Specimen>()
            .Include(specimen => specimen.Organoid)
            .Where(specimen => specimen.Organoid != null)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withXenografts = _dbContext.Set<Specimen>()
            .Include(specimen => specimen.Xenograft)
            .Where(specimen => specimen.Xenograft != null)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withSimpleSomaticMutations = _dbContext.Set<SSM.VariantOccurrence>()
            .Where(occurrence => donorIds.Contains(occurrence.AnalysedSample.Sample.Specimen.DonorId))
            .Select(occurrence => occurrence.AnalysedSample.Sample.Specimen.DonorId)
            .Distinct()
            .Count();

        var withCopyNumberVariants = _dbContext.Set<CNV.VariantOccurrence>()
            .Where(occurrence => donorIds.Contains(occurrence.AnalysedSample.Sample.Specimen.DonorId))
            .Select(occurrence => occurrence.AnalysedSample.Sample.Specimen.DonorId)
            .Distinct()
            .Count();

        var withStructuralVariants = _dbContext.Set<SV.VariantOccurrence>()
            .Where(occurrence => donorIds.Contains(occurrence.AnalysedSample.Sample.Specimen.DonorId))
            .Select(occurrence => occurrence.AnalysedSample.Sample.Specimen.DonorId)
            .Distinct()
            .Count();

        var withGeneExpressions = _dbContext.Set<GeneExpression>()
            .Where(geneExpression => donorIds.Contains(geneExpression.AnalysedSample.Sample.Specimen.DonorId))
            .Select(geneExpression => geneExpression.AnalysedSample.Sample.Specimen.DonorId)
            .Distinct()
            .Count();


        return new ProjectDataModel
        {
            Total = donorIds.Length,
            MRI = withMriImages,
            CT = withCtImages,
            Tissues = withTissues,
            Cells = withCellLines,
            Organoids = withOrganoids,
            Xenografts = withXenografts,
            SSM = withSimpleSomaticMutations,
            CNV = withCopyNumberVariants,
            SV = withStructuralVariants,
            TRA = withGeneExpressions
        };
    }
}
