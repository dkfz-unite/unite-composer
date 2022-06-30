using IDomainSqlOptions = Unite.Data.Services.Configuration.Options.ISqlOptions;
using IIdentitySqlOptions = Unite.Identity.Services.Configuration.Options.ISqlOptions;

namespace Unite.Composer.Web.Configuration.Options;

public class SqlOptions : IIdentitySqlOptions, IDomainSqlOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_SQL_HOST");
    public string Port => Environment.GetEnvironmentVariable("UNITE_SQL_PORT");
    public string User => Environment.GetEnvironmentVariable("UNITE_SQL_USER");
    public string Password => Environment.GetEnvironmentVariable("UNITE_SQL_PASSWORD");
}
