using System;
using Unite.Indices.Entities.Basic.Clinical;

namespace Unite.Composer.Web.Resources.Donors
{
    public class TreatmentResource
    {
        public string Therapy { get; set; }
        public string Details { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? ProgressionStatus { get; set; }
        public DateTime? ProgressionStatusChangeDate { get; set; }
        public string Results { get; set; }

        public TreatmentResource(TreatmentIndex index)
        {
            Therapy = index.Therapy;
            Details = index.Details;
            StartDate = index.StartDate;
            EndDate = index.EndDate;
            ProgressionStatus = index.ProgressionStatus;
            ProgressionStatusChangeDate = index.ProgressionStatusChangeDate;
            Results = index.Results;
        }
    }
}
