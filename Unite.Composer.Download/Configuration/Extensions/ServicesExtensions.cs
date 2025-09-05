using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Download.Services.Tsv;

namespace Unite.Composer.Download.Configuration.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddTsvDownload(this IServiceCollection services)
    {
        services.AddTransient<DonorsDownloadService>();
        services.AddTransient<ImagesDownloadService>();
        services.AddTransient<SpecimensDownloadService>();

        return services;
    }
}
