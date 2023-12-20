using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Download.Tsv.Mapping;

namespace Unite.Composer.Download.Configuration.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddTsvMapping(this IServiceCollection services)
    {
        services.AddTransient<DonorsTsvService>();
        services.AddTransient<ImagesTsvService>();
        services.AddTransient<SpecimensTsvService>();
        services.AddTransient<VariantsTsvService>();
        services.AddTransient<TranscriptomicsTsvService>();

        return services;
    }

    public static IServiceCollection AddTsvDownload(this IServiceCollection services)
    {
        services.AddTsvMapping();

        services.AddTransient<DonorsTsvDownloadService>();
        services.AddTransient<ImagesTsvDownloadService>();
        services.AddTransient<SpecimensTsvDownloadService>();
        services.AddTransient<GenesTsvDownloadService>();
        services.AddTransient<VariantsTsvDownloadService>();

        return services;
    }
}
