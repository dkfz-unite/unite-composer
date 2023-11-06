using Unite.Composer.Analysis.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Options;

public class AnalysisOptions : IAnalysisOptions
{
    public string DataPath
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_ANALYSIS_DATA_PATH");

            if (string.IsNullOrWhiteSpace(option))
                throw new ArgumentNullException("'UNITE_ANALYSIS_DATA_PATH' environment variable has to be set");

            return option.Trim();
        }
    }

    public string DESeq2Url
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_ANALYSIS_DESEQ2_URL");

            if (string.IsNullOrWhiteSpace(option))
                throw new ArgumentNullException("'UNITE_ANALYSIS_DESEQ2_URL' environment variable has to be set");

            return option.Trim();
        }
    }

    public int? Limit { get; }
}
