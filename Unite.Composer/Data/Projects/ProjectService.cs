using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Projects.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Enums;

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
            .AsNoTracking()
            .FirstOrDefault(project => project.Id == id);

        if (project != null)
        {
            var projectDonorIds = _dbContext.Set<ProjectDonor>()
                .AsNoTracking()
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
            .AsNoTracking()
            .OrderBy(project => project.Name)
            .ToArray();

        foreach (var project in projects)
        {
            var projectDonorIds = _dbContext.Set<ProjectDonor>()
                .AsNoTracking()
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
            .AsNoTracking()
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
            .AsNoTracking()
            .Where(image => image.TypeId == ImageType.MRI)
            .Where(image => donorIds.Contains(image.DonorId))
            .Select(image => image.DonorId)
            .Distinct()
            .Count();

        var withCtImages = 0;

        var withTissues = _dbContext.Set<Specimen>()
            .AsNoTracking()
            .Where(specimen => specimen.TypeId == SpecimenType.Tissue)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withCellLines = _dbContext.Set<Specimen>()
            .AsNoTracking()
            .Where(specimen => specimen.TypeId == SpecimenType.CellLine)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withOrganoids = _dbContext.Set<Specimen>()
            .AsNoTracking()
            .Where(specimen => specimen.TypeId == SpecimenType.Organoid)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withXenografts = _dbContext.Set<Specimen>()
            .AsNoTracking()
            .Where(specimen => specimen.TypeId == SpecimenType.Xenograft)
            .Where(specimen => donorIds.Contains(specimen.DonorId))
            .Select(specimen => specimen.DonorId)
            .Distinct()
            .Count();

        var withSimpleSomaticMutations = _dbContext.Set<SSM.VariantEntry>()
            .AsNoTracking()
            .Where(occurrence => donorIds.Contains(occurrence.AnalysedSample.TargetSample.DonorId))
            .Select(occurrence => occurrence.AnalysedSample.TargetSample.DonorId)
            .Distinct()
            .Count();

        var withCopyNumberVariants = _dbContext.Set<CNV.VariantEntry>()
            .AsNoTracking()
            .Where(occurrence => donorIds.Contains(occurrence.AnalysedSample.TargetSample.DonorId))
            .Select(occurrence => occurrence.AnalysedSample.TargetSample.DonorId)
            .Distinct()
            .Count();

        var withStructuralVariants = _dbContext.Set<SV.VariantEntry>()
            .AsNoTracking()
            .Where(occurrence => donorIds.Contains(occurrence.AnalysedSample.TargetSample.DonorId))
            .Select(occurrence => occurrence.AnalysedSample.TargetSample.DonorId)
            .Distinct()
            .Count();

        var withGeneExpressions = _dbContext.Set<BulkExpression>()
            .AsNoTracking()
            .Where(geneExpression => donorIds.Contains(geneExpression.AnalysedSample.TargetSample.DonorId))
            .Select(geneExpression => geneExpression.AnalysedSample.TargetSample.DonorId)
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
