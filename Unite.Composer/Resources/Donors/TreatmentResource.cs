using Unite.Indices.Entities;

namespace Unite.Composer.Resources.Donors
{
    public class TreatmentResource
    {
        public TherapyResource Therapy { get; set; }

        public string Details { get; set; }
        public string Results { get; set; }

        public TreatmentResource(TreatmentIndex index)
        {
            Therapy = new TherapyResource(index.Therapy);

            Details = index.Details;
            Results = index.Results;
        }
    }
}
