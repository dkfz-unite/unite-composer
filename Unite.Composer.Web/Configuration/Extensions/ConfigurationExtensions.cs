using FluentValidation;
using Unite.Composer.Admin.Services;
using Unite.Composer.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Data.Donors;
using Unite.Composer.Data.Genome;
using Unite.Composer.Data.Genome.Ranges;
using Unite.Composer.Data.Images;
using Unite.Composer.Data.Projects;
using Unite.Composer.Data.Specimens;
using Unite.Composer.Identity.Services;
using Unite.Composer.Search.Services;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.HostedServices;
using Unite.Composer.Web.Models.Admin;
using Unite.Composer.Web.Models.Admin.Validators;
using Unite.Composer.Web.Models.Identity;
using Unite.Composer.Web.Models.Identity.Validators;
using Unite.Data.Services;
using Unite.Identity.Services;
using Unite.Indices.Services.Configuration.Options;

using IDomainSqlOptions = Unite.Data.Services.Configuration.Options.ISqlOptions;
using IIdentitySqlOptions = Unite.Identity.Services.Configuration.Options.ISqlOptions;

namespace Unite.Composer.Web.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddValidation();

        services.AddTransient<IdentityDbContext>();
        services.AddTransient<DomainDbContext>();

        services.AddTransient<UserService>();
        services.AddTransient<IdentityService>();
        services.AddTransient<SessionService>();

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

        services.AddTransient<ProjectService>();
        services.AddTransient<DrugScreeningService>();
        services.AddTransient<GenomicProfileService>();
        services.AddTransient<OncoGridDataService>();
        services.AddTransient<OncoGridDataService1>();
        services.AddTransient<ProteinPlotDataService>();

        services.AddHostedService<RootHostedService>();
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services.AddTransient<ApiOptions>();
        services.AddTransient<RootOptions>();
        services.AddTransient<IElasticOptions, ElasticOptions>();
        services.AddTransient<IIdentitySqlOptions, SqlOptions>();
        services.AddTransient<IDomainSqlOptions, SqlOptions>();
        services.AddTransient<IEnsemblOptions, EnsemblOptions>();
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddTransient<IValidator<AddUserModel>, AddUserModelValidator>();
        services.AddTransient<IValidator<EditUserModel>, EditUserModelValidator>();
        services.AddTransient<IValidator<SignUpModel>, SignUpModelValidator>();
        services.AddTransient<IValidator<SignInModel>, SignInModelValidator>();
        services.AddTransient<IValidator<PasswordChangeModel>, PasswordChangeModelValidator>();
    }
}
