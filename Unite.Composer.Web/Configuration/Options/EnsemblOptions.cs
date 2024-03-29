﻿using Unite.Composer.Clients.Ensembl.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Options;

public class EnsemblOptions : IEnsemblOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_ENSEMBL_HOST");
}
