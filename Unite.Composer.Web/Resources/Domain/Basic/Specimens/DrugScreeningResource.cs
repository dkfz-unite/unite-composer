using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens.Drugs;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class DrugScreeningResource
{
    public string Drug { get; set; }
    public double? Gof { get; set; }
    public double? Dss { get; set; }
    public double? DssS { get; set; }
    public double? MinDose { get; set; }
    public double? MaxDose { get; set; }
    public double? Dose25 { get; set; }
    public double? Dose50 { get; set; }
    public double? Dose75 { get; set; }
    public double[] Doses { get; set; }
    public double[] Responses { get; set; }


    /// <summary>
    /// Initialises drug screening resource from index.
    /// Does not include drug response curve data.
    /// </summary>
    /// <param name="index">Drug screening index</param>
    public DrugScreeningResource(DrugScreeningIndex index)
    {
        Drug = index.Drug;
        Gof = index.Gof;
        Dss = index.Dss;
        DssS = index.DssS;
        MinDose = index.MinDose;
        MaxDose = index.MaxDose;
        Dose25 = index.Dose25;
        Dose50 = index.Dose50;
        Dose75 = index.Dose75;
    }

    /// <summary>
    /// Initialises drug screening resource from database model.
    /// Includes drug response curve data.
    /// </summary>
    /// <param name="model">Drug screening model</param>
    public DrugScreeningResource(DrugScreeningModel model)
    {
        Drug = model.Drug;
        Gof = model.Gof;
        Dss = model.Dss;
        DssS = model.DssS;
        MinDose = model.MinDose;
        MaxDose = model.MaxDose;
        Dose25 = model.Dose25;
        Dose50 = model.Dose50;
        Dose75 = model.Dose75;
        Doses = model.Doses;
        Responses = model.Responses;
    }
}
