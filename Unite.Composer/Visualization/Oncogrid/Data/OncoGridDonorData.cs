using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.OncoGrid
{
    public class OncoGridDonorResource
    {
        /// <summary>
        /// Attention: Do not rename this attribute because this name is required within the used javascript framework for oncogrid:
        /// https://github.com/oncojs/oncogrid
        /// </summary>
        public string Id { get; }
        public string Gender { get; }
        public int? Age { get; }
        public bool? VitalStatus { get; }
        public int Mutations { get; }
        public int Genes { get; }

        public OncoGridDonorResource(DonorIndex index)
        {
            Id = index.ReferenceId;

            if (index.ClinicalData != null)
            {
                Gender = index.ClinicalData.Gender;
                Age = index.ClinicalData.Age;
                VitalStatus = index.ClinicalData.VitalStatus;
            }
            Mutations = index.NumberOfMutations;
            Genes = index.NumberOfGenes;
        }
    }
}