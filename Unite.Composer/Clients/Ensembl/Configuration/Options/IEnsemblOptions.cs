namespace Unite.Composer.Clients.Ensembl.Configuration.Options;

public interface IEnsemblOptions
{
    string Host { get; }

    /// <summary>
    /// Genome build version of the installation (37 or 38).
    /// </summary>
    byte Grch { get; }
}
