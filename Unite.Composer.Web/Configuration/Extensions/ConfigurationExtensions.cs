using Microsoft.EntityFrameworkCore;
using Unite.Composer.Admin.Services;
using Unite.Composer.Analysis.Configuration.Options;
using Unite.Composer.Analysis.Expression;
using Unite.Composer.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Data.Donors;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Images;
using Unite.Composer.Data.Projects;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Composer.Search.Services;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.Handlers;
using Unite.Composer.Web.HostedServices;
using Unite.Composer.Web.Services;
using Unite.Data.Services;
using Unite.Data.Services.Configuration.Options;
using Unite.Indices.Services.Configuration.Options;


namespace Unite.Composer.Web.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddValidation();

        AddDatabase(services);

        services.AddTransient<TaskStatsService>();

        services.AddTransient<IDonorsSearchService, DonorsSearchService>();
        services.AddTransient<IImagesSearchService, ImagesSearchService>();
        services.AddTransient<ISpecimensSearchService, SpecimensSearchService>();
        services.AddTransient<IGenesSearchService, GenesSearchService>();
        services.AddTransient<IVariantsSearchService, VariantsSearchService>();

        services.AddTransient<DonorDataService>();
        services.AddTransient<ImageDataService>();
        services.AddTransient<SpecimenDataService>();
        services.AddTransient<SampleDataService>();
        services.AddTransient<GeneDataService>();
        services.AddTransient<MutationDataService>();
        
        services.AddTransient<DonorsTsvService>();
        services.AddTransient<ImagesTsvService>();
        services.AddTransient<SpecimensTsvService>();
        services.AddTransient<VariantsTsvService>();
        services.AddTransient<TranscriptomicsTsvService>();
        services.AddTransient<DonorsTsvDownloadService>();
        services.AddTransient<ImagesTsvDownloadService>();
        services.AddTransient<SpecimensTsvDownloadService>();
        services.AddTransient<GenesTsvDownloadService>();
        services.AddTransient<VariantsTsvDownloadService>();

        services.AddTransient<ProjectService>();
        services.AddTransient<DrugScreeningService>();
        services.AddTransient<GenomicProfileService>();
        services.AddTransient<OncoGridDataService>();
        services.AddTransient<OncoGridDataService1>();
        services.AddTransient<ProteinPlotDataService>();

        services.AddHostedService<AnalysisPreparingHostedService>();
        services.AddTransient<AnalysisPreparingHandler>();
        services.AddTransient<AnalysisTaskService>();
        services.AddTransient<ExpressionAnalysisService>();

        services.AddHostedService<AnalysisProcessingHostedService>();
        services.AddTransient<AnalysisProcessingHandler>();
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services.AddTransient<ApiOptions>();
        services.AddTransient<IElasticOptions, ElasticOptions>();
        services.AddTransient<ISqlOptions, SqlOptions>();
        services.AddTransient<IEnsemblOptions, EnsemblOptions>();
        services.AddTransient<IAnalysisOptions, AnalysisOptions>(); 
        services.AddTransient<AnalysisOptions>();
    }

    private static void AddValidation(this IServiceCollection services)
    {

    }

    private static void AddDatabase(this IServiceCollection services)
    {
        var options = new SqlOptions();
        var database = "unite_data";
        var connectionString = $"Host={options.Host};Port={options.Port};Database={database};Username={options.User};Password={options.Password}";

        services.AddTransient<DomainDbContext>();

        services.AddDbContextFactory<DomainDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }
}
