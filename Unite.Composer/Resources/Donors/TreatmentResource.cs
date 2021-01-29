using System;
using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Resources.Donors
{
    public class TreatmentResource
    {
        public string Therapy { get; set; }
        public string Details { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Results { get; set; }

        public TreatmentResource(TreatmentIndex index)
        {
            Therapy = index.Therapy;
            Details = index.Details;
            StartDate = index.StartDate;
            EndDate = index.EndDate;
            Results = index.Results;
        }
    }
}
