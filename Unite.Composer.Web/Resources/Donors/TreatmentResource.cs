using Unite.Indices.Entities.Basic.Donors.Clinical;

namespace Unite.Composer.Web.Resources.Donors;

public class TreatmentResource
{
    public string Therapy { get; set; }
    public string Details { get; set; }
    public int? StartDay { get; set; }
    public int? DurationDays { get; set; }
    public string Results { get; set; }

    public TreatmentResource(TreatmentIndex index)
    {
        Therapy = index.Therapy;
        Details = index.Details;
        StartDay = index.StartDay;
        DurationDays = index.DurationDays;
        Results = index.Results;
    }
}
