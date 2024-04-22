using Microsoft.AspNetCore.ResponseCompression;

namespace Unite.Composer.Web.Configuration.Extensions;

public static class CompressionExtensions
{
    public static void AddCompressionOptions(this ResponseCompressionOptions options)
    {
        options.EnableForHttps = true;
        options.Providers.Add<GzipCompressionProvider>();
    }
}
