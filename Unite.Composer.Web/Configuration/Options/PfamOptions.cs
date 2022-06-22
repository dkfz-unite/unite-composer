using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Options;

public class PfamOptions : IPfamOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_PFAM_HOST");
}
