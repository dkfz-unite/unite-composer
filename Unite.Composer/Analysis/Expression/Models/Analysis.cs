using Unite.Composer.Analysis.Models;

namespace Unite.Composer.Analysis.Expression.Models;

public record Analysis
{
    /// <summary>
    /// Analysis key. Used to identify the analysis in the queue and UI.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Datasets to analyse.
    /// </summary>
    public DatasetCriteria[] Cohorts { get; set; }
}
