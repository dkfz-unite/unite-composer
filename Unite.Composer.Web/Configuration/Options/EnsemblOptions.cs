using Unite.Composer.Clients.Ensembl.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Options;

public class EnsemblOptions : IEnsemblOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_ENSEMBL_HOST");

    public byte Grch
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_GENOME_BUILD");

            if (string.IsNullOrWhiteSpace(option))
                return 37;

            return option.Trim().ToLowerInvariant() switch
            {
                "grch37" => 37,
                "grch38" => 38,
                _ => throw new ArgumentException($"Invalid genome build: {option}. Must be one of: GRCh37, GRCh38")
            };
        }
    }
}
