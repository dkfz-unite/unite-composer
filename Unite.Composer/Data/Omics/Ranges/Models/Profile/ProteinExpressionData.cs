using System.Text.Json.Serialization;

namespace Unite.Composer.Data.Omics.Ranges.Models.Profile;

public class ProteinExpressionData : RangeData
{
    /// <summary>
    /// Protein entry.
    /// </summary>
    [JsonPropertyName("e")]
    public ProteinExpression Expression { get; set; }

    /// <summary>
    /// Array of protein expression stats in format [Raw, MedianCenteredLog2].
    /// </summary> 
    public double[] Intensity { get; set; }


    public ProteinExpressionData(int[] range, Unite.Data.Entities.Omics.Analysis.Prot.ProteinExpression expression) : base(range)
    {
        Expression = new ProteinExpression(expression);

        var raw = Math.Round((double)expression.Intensity);
        var medianCenteredLog2 = Math.Round((double)expression.MedianCenteredLog2, 3);

        Intensity = [raw, medianCenteredLog2];
    }

    public ProteinExpressionData(int[] range, IEnumerable<Unite.Data.Entities.Omics.Analysis.Prot.ProteinExpression> expressions) : base(range)
    {
        foreach (var expression in expressions)
        {
            var raw = Math.Round(expressions.Average(expression => expression.Intensity));
            var medianCenteredLog2 = Math.Round(expressions.Average(expression => expression.MedianCenteredLog2), 3);

            Intensity = [raw, medianCenteredLog2];
        }
    }   
}

public class ProteinExpression
{
    public string Protein { get; set; }

    public ProteinExpression(Unite.Data.Entities.Omics.Analysis.Prot.ProteinExpression expression)
    {
        Protein = expression.Entity.Symbol;
    }
}
