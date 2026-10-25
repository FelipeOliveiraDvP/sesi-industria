namespace Catalog.Api.DTOs;

public sealed record CourseSummaryDto(
    int Id,
    string FullName,
    string ShortName,
    string Description,
    string ShortDescription,
    LookupDto Category,
    LookupDto Modality,
    LookupDto Status,
    decimal WorkloadHours,
    string TargetAudience,
    string ImageUrl,
    DateOnly? AvailableOn);
