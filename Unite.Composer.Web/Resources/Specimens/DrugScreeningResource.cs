using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class DrugScreeningResource
{
    public string Drug { get; set; }
    public double? Dss { get; set; }
    public double? DssSelective { get; set; }
    public double? Gof { get; set; }
    public double? MinConcentration { get; set; }
    public double? MaxConcentration { get; set; }
    public double? AbsIC25 { get; set; }
    public double? AbsIC50 { get; set; }
    public double? AbsIC75 { get; set; }
    public double[] Inhibition { get; set; }


    public DrugScreeningResource(DrugScreeningIndex index)
    {
        Drug = index.Drug;
        Dss = index.Dss;
        DssSelective = index.DssSelective;
        Gof = index.Gof;
        MinConcentration = index.MinConcentration;
        MaxConcentration = index.MaxConcentration;
        AbsIC25 = index.AbsIC25;
        AbsIC50 = index.AbsIC50;
        AbsIC75 = index.AbsIC75;
        Inhibition = index.Inhibition;
    }
}
