namespace Catalog.Api.Services;

public static class CourseSearch
{
    public static string? CreatePattern(string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return null;
        }

        var literal = search.Trim()
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);
        return $"%{literal}%";
    }
}
