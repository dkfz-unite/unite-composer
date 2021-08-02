﻿using System.Linq;
using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Web.Resources.Donors
{
    public class DonorBaseResource
    {
        public int Id { get; set; }
        public string ReferenceId { get; set; }
        public bool? MtaProtected { get; set; }

        public ClinicalDataResource ClinicalData { get; set; }
        public TreatmentResource[] Treatments { get; set; }
        public WorkPackageResource[] WorkPackages { get; set; }
        public StudyResource[] Studies { get; set; }


        public DonorBaseResource(DonorIndex index)
        {
            Id = index.Id;
            ReferenceId = index.ReferenceId;
            MtaProtected = index.MtaProtected;

            if (index.ClinicalData != null)
            {
                ClinicalData = new ClinicalDataResource(index.ClinicalData);
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
        }
    }
}
