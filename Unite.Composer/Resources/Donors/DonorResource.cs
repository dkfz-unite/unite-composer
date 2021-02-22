using System;
using System.Linq;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Resources.Donors
{
    public class DonorResource
    {
        public string Id { get; set; }
        public string Origin { get; set; }
        public bool? MtaProtected { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? DiagnosisDate { get; set; }

        public ClinicalDataResource ClinicalData { get; set; }
        public EpigeneticsDataResource EpigeneticsData { get; set; }
        public TreatmentResource[] Treatments { get; set; }
        public WorkPackageResource[] WorkPackages { get; set; }
        public StudyResource[] Studies { get; set; }

        public int Samples { get; set; }
        public int Mutations { get; set; }
        public int Genes { get; set; }

        public DonorResource(DonorIndex index)
        {
            Id = index.Id;
            Origin = index.Origin;
            MtaProtected = index.MtaProtected;
            Diagnosis = index.Diagnosis;
            DiagnosisDate = index.DiagnosisDate;

            if (index.ClinicalData != null)
            {
                ClinicalData = new ClinicalDataResource(index.ClinicalData);
            }

            if(index.EpigeneticsData != null)
            {
                EpigeneticsData = new EpigeneticsDataResource(index.EpigeneticsData);
            }

            if (index.Treatments != null && index.Treatments.Any())
            {
                Treatments = index.Treatments
                    .Select(treatment => new TreatmentResource(treatment))
                    .ToArray();
            }

            if (index.WorkPackages != null && index.WorkPackages.Any())
            {
                WorkPackages = index.WorkPackages
                    .Select(workPackage => new WorkPackageResource(workPackage))
                    .ToArray();
            }

            if (index.Studies != null && index.Studies.Any())
            {
                Studies = index.Studies
                    .Select(study => new StudyResource(study))
                    .ToArray();
            }

            

            if (index.Mutations != null && index.Mutations.Any())
            {
                Samples = index.Mutations
                    .SelectMany(mutation => mutation.Samples)
                    .GroupBy(sample => sample.Id)
                    .Select(g => g.First())
                    .Count();
            }

            Mutations = index.NumberOfMutations;
            Genes = index.NumberOfGenes;
        }
    }
}
