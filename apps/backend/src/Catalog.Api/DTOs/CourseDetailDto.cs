namespace Catalog.Api.DTOs;

public sealed record CourseDetailDto(
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
    DateOnly? AvailableOn,
    IReadOnlyList<CourseModuleDto> Modules,
    CoursePresentationDto Presentation);
