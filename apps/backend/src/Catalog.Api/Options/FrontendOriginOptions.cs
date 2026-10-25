namespace Catalog.Api.Options;

public sealed class FrontendOriginOptions
{
    public const string ConfigurationKey = "FRONTEND_URL";

    public string Value { get; set; } = string.Empty;
}

public static class FrontendOrigin
{
    public static bool TryParse(string? value, out Uri? origin)
    {
        origin = null;
        if (string.IsNullOrWhiteSpace(value) || !string.Equals(value, value.Trim(), StringComparison.Ordinal) ||
            value.Contains('?') || value.Contains('#') ||
            !Uri.TryCreate(value, UriKind.Absolute, out var parsed))
            return false;

        if ((parsed.Scheme != Uri.UriSchemeHttp && parsed.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(parsed.Host) || parsed.UserInfo.Length > 0 ||
            parsed.Authority.Contains('@') || parsed.AbsolutePath != "/")
            return false;

        origin = parsed;
        return true;
    }

    public static string Normalize(string value)
    {
        if (!TryParse(value, out var origin))
            throw new ArgumentException("FRONTEND_URL must be an absolute HTTP or HTTPS origin.", nameof(value));

        return origin!.GetLeftPart(UriPartial.Authority);
    }
}
