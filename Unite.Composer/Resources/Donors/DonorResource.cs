using System;
using System.Collections.Generic;
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
        public TreatmentResource[] Treatments { get; set; }
        public WorkPackageResource[] WorkPackages { get; set; }
        public StudyResource[] Studies { get; set; }

        public int CellLines { get; set; }
        public int Samples { get; set; }
        public int Mutations { get; set; }
        public int Genes { get; set; }

        public DonorResource(DonorIndex index)
        {
            var mutations = new List<int>();
            var genes = new List<int>();

            Id = index.Id;
            Origin = index.Origin;
            MtaProtected = index.MtaProtected;
            Diagnosis = index.Diagnosis;
            DiagnosisDate = index.DiagnosisDate;

            if(index.ClinicalData != null)
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

            if(index.Studies != null && index.Studies.Any())
            {
                Studies = index.Studies
                    .Select(study => new StudyResource(study))
                    .ToArray();
            }

            if (index.CellLines != null && index.CellLines.Any())
            {
                CellLines = index.CellLines
                    .Select(cellLine => cellLine.Id)
                    .Distinct()
                    .Count();

                var cellLineMutations = index.CellLines
                    .Where(cellLine => cellLine.Samples != null)
                    .SelectMany(cellLine => cellLine.Samples)
                    .Where(sample => sample.Mutations != null)
                    .SelectMany(sample => sample.Mutations)
                    .Select(mutation => mutation.Id)
                    .Distinct();

                if (cellLineMutations.Any())
                {
                    mutations.AddRange(cellLineMutations);
                }

                var cellLineGenes = index.CellLines
                    .Where(cellLine => cellLine.Samples != null)
                    .SelectMany(cellLine => cellLine.Samples)
                    .Where(sample => sample.Mutations != null)
                    .SelectMany(sample => sample.Mutations)
                    .Where(mutation => mutation.Gene != null)
                    .Select(mutation => mutation.Gene.Id)
                    .Distinct();

                if (cellLineGenes.Any())
                {
                    genes.AddRange(cellLineGenes);
                }
            }

            if (index.Samples != null && index.Samples.Any())
            {
                Samples = index.Samples
                    .Select(sample => sample.Id)
                    .Distinct()
                    .Count();

                var sampleMutations = index.Samples
                    .Where(sample => sample.Mutations != null)
                    .SelectMany(sample => sample.Mutations)
                    .Select(mutation => mutation.Id)
                    .Distinct();

                if (sampleMutations.Any())
                {
                    mutations.AddRange(sampleMutations);
                }

                var sampleGenes = index.Samples
                    .Where(sample => sample.Mutations != null)
                    .SelectMany(sample => sample.Mutations)
                    .Where(mutation => mutation.Gene != null)
                    .Select(mutation => mutation.Gene.Id)
                    .Distinct();

                if (sampleGenes.Any())
                {
                    genes.AddRange(sampleGenes);
                }
            }

            if (mutations.Any())
            {
                Mutations = mutations
                    .Distinct()
                    .Count();
            }

            if (genes.Any())
            {
                Genes = genes
                    .Distinct()
                    .Count();
            }
        }
    }
}
