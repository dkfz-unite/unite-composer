using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Web.Resources.Domain.Basic.Donors;

public class DonorResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public bool? MtaProtected { get; set; }

    public ClinicalDataResource ClinicalData { get; set; }
    public TreatmentResource[] Treatments { get; set; }
    public ProjectResource[] Projects { get; set; }
    public StudyResource[] Studies { get; set; }


    public DonorResource(DonorIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        MtaProtected = index.MtaProtected;
        
        if (index.ClinicalData != null)
            ClinicalData = new ClinicalDataResource(index.ClinicalData);

        if (index.Treatments.IsNotEmpty())
        {
            Treatments = index.Treatments
                .Select(treatment => new TreatmentResource(treatment))
                .ToArray();
        }

        if (index.Projects.IsNotEmpty())
        {
            Projects = index.Projects
                .Select(project => new ProjectResource(project))
                .ToArray();
        }

        if (index.Studies.IsNotEmpty())
        {
            Studies = index.Studies
                .Select(study => new StudyResource(study))
                .ToArray();
        }
    }
}
