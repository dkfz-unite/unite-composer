using Unite.Cache.Configuration.Options;
using Unite.Composer.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Data.Datasets;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Data.Submissions;
using Unite.Composer.Download.Configuration.Extensions;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Web.Configuration.Options;
using Unite.Data.Context.Configuration.Extensions;
using Unite.Data.Context.Configuration.Options;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Configuration.Extensions;


namespace Unite.Composer.Web.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        var sqlOptions = new SqlOptions();

        services.AddOptions();
        services.AddDatabase();
        services.AddDatabaseFactory(sqlOptions);
        services.AddSearchEngine();
        services.AddSearchServices();
        services.AddValidation();

        services.AddTransient<Admin.Services.TaskStatsService>();
        services.AddTransient<Admin.Services.SubmissionsService>();

        services.AddTransient<DrugScreeningService>();
        services.AddTransient<GeneDataService>();
        services.AddTransient<SmDataService>();
        services.AddTransient<SubmissionsService>();
        services.AddTransient<DatasetsService>();
        services.AddTransient<DatasetService>();

        services.AddTsvDownload();

        services.AddTransient<GenomicProfileService>();
        services.AddTransient<OncoGridDataService>();
        services.AddTransient<ProteinPlotDataService>();
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services.AddTransient<ApiOptions>();
        services.AddTransient<IElasticOptions, ElasticOptions>();
        services.AddTransient<ISqlOptions, SqlOptions>();
        services.AddTransient<IMongoOptions, MongoOptions>();
        services.AddTransient<IEnsemblOptions, EnsemblOptions>();
    }

    private static void AddValidation(this IServiceCollection services)
    {

    }
}
