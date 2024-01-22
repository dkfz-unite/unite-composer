using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class DrugScreeningResource
{
    public string Drug { get; set; }
    public double? Dss { get; set; }
    public double? DssSelective { get; set; }
    public double? Gof { get; set; }
    public double? AbsIC25 { get; set; }
    public double? AbsIC50 { get; set; }
    public double? AbsIC75 { get; set; }
    public double? MinConcentration { get; set; }
    public double? MaxConcentration { get; set; }
    public double[] Concentration { get; set; }
    public double[] Inhibition { get; set; }
    public double[] ConcentrationLine { get; set; }
    public double[] InhibitionLine { get; set; }


    /// <summary>
    /// Initialises drug screening resource from index.
    /// Does not include drug response curve data.
    /// </summary>
    /// <param name="index">Drug screening index</param>
    public DrugScreeningResource(DrugScreeningIndex index)
    {
        Drug = index.Drug;
        Dss = index.Dss;
        DssSelective = index.DssSelective;
        Gof = index.Gof;
        AbsIC25 = index.AbsIC25;
        AbsIC50 = index.AbsIC50;
        AbsIC75 = index.AbsIC75;
        MinConcentration = index.MinConcentration;
        MaxConcentration = index.MaxConcentration;
    }

    /// <summary>
    /// Initialises drug screening resource from database model.
    /// Includes drug response curve data.
    /// </summary>
    /// <param name="model">Drug screening model</param>
    public DrugScreeningResource(DrugScreeningModel model)
    {
        Drug = model.Drug;
        Dss = model.Dss;
        DssSelective = model.DssSelective;
        Gof = model.Gof;
        AbsIC25 = model.AbsIC25;
        AbsIC50 = model.AbsIC50;
        AbsIC75 = model.AbsIC75;
        MinConcentration = model.MinConcentration;
        MaxConcentration = model.MaxConcentration;
        Concentration = model.Concentration;
        Inhibition = model.Inhibition;
        ConcentrationLine = model.ConcentrationLine;
        InhibitionLine = model.InhibitionLine;
    }
}
