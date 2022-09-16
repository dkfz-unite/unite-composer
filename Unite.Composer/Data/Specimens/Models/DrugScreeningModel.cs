namespace Unite.Composer.Data.Specimens.Models;

public class DrugScreeningModel
{
    public string Drug;
    public double? Dss;
    public double? DssSelective;
    public double? Gof;
    public double? MinConcentration;
    public double? MaxConcentration;
    public double? AbsIC25;
    public double? AbsIC50;
    public double? AbsIC75;
    public double[] Concentration;
    public double[] Inhibition;
    public double[] Dose;
    public double[] Response;
}
