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
        {
            ClinicalData = new ClinicalDataResource(index.ClinicalData);
        }

        if (index.Treatments?.Any() == true)
        {
            Treatments = index.Treatments
                .Select(treatment => new TreatmentResource(treatment))
                .ToArray();
        }

        if (index.Projects?.Any() == true)
        {
            Projects = index.Projects
                .Select(project => new ProjectResource(project))
                .ToArray();
        }

        if (index.Studies?.Any() == true)
        {
            Studies = index.Studies
                .Select(study => new StudyResource(study))
                .ToArray();
        }
    }
}
