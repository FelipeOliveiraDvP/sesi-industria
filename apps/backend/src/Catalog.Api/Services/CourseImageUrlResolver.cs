using Catalog.Api.Options;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Services;

public sealed class CourseImageUrlResolver(IOptions<FrontendOriginOptions> options)
{
    private readonly string frontendBaseUrl = FrontendOrigin.Normalize(options.Value.Value);

    public string Resolve(string publicPath)
    {
        if (string.IsNullOrWhiteSpace(publicPath) ||
            !publicPath.StartsWith("/assets/", StringComparison.Ordinal) ||
            publicPath.Contains("..", StringComparison.Ordinal) ||
            publicPath.Contains('\\') || publicPath.Contains('?') || publicPath.Contains('#') ||
            publicPath.Contains(':') || publicPath.Contains('%') || publicPath.Contains("//", StringComparison.Ordinal))
            throw new InvalidOperationException("Course image paths must refer to a local asset under /assets/.");

        return string.Concat(frontendBaseUrl, publicPath);
    }
}
