using System.Text.Json.Serialization;

namespace Unite.Composer.Data.Omics.Ranges.Models.Profile;

public class ExpressionData : RangeData
{
    /// <summary>
    /// Gene entry.
    /// </summary>
    [JsonPropertyName("e")]
    public Expression Expression { get; set; }

    /// <summary>
    /// Array of gene expression stats in format [Reads, TPM, FPKM].
    /// </summary> 
    public double[] Reads { get; set; }


    public ExpressionData(int[] range, Unite.Data.Entities.Omics.Analysis.Rna.GeneExpression expression) : base(range)
    {
        Expression = new Expression(expression);

        var reads = Math.Round((double)expression.Reads);
        var tpm = Math.Round((double)expression.TPM);
        var fpkm = Math.Round((double)expression.FPKM);

        Reads = [reads, tpm, fpkm];
    }

    public ExpressionData(int[] range, IEnumerable<Unite.Data.Entities.Omics.Analysis.Rna.GeneExpression> expressions) : base(range)
    {
        foreach (var expression in expressions)
        {
            var reads = Math.Round(expressions.Average(expression => expression.Reads));
            var tpm = Math.Round(expressions.Average(expression => expression.TPM));
            var fpkm = Math.Round(expressions.Average(expression => expression.FPKM));

            Reads = [reads, tpm, fpkm];
        }
    }
}

public class Expression
{
    public string Gene { get; set; }

    public Expression(Unite.Data.Entities.Omics.Analysis.Rna.GeneExpression expression)
    {
        Gene = expression.Entity.Symbol;
    }
}
