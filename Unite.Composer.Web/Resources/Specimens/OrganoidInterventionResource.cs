using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class OrganoidInterventionResource
{
    public string Type { get; set; }
    public string Details { get; set; }
    public int? StartDay { get; set; }
    public int? DurationDays { get; set; }
    public string Results { get; set; }


    public OrganoidInterventionResource(OrganoidInterventionIndex index)
    {
        Type = index.Type;
        Details = index.Details;
        StartDay = index.StartDay;
        DurationDays = index.DurationDays;
        Results = index.Results;
    }
}
