﻿using Unite.Indices.Entities.Basic.Clinical;

namespace Unite.Composer.Web.Resources.Donors
{
    public class TreatmentResource
    {
        public string Therapy { get; set; }
        public string Details { get; set; }
        public int? StartDay { get; set; }
        public int? DurationDays { get; set; }
        public bool? ProgressionStatus { get; set; }
        public int? ProgressionStatusChangeDay { get; set; }
        public string Results { get; set; }

        public TreatmentResource(TreatmentIndex index)
        {
            Therapy = index.Therapy;
            Details = index.Details;
            StartDay = index.StartDay;
            DurationDays = index.DurationDays;
            ProgressionStatus = index.ProgressionStatus;
            ProgressionStatusChangeDay = index.ProgressionStatusChangeDay;
            Results = index.Results;
        }
    }
}
