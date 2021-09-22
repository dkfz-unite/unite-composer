using System;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Options
{
    public class UniprotOptions : IUniprotOptions
    {
        public string Host => Environment.GetEnvironmentVariable("UNITE_UNIPROT_HOST");
    }
}
