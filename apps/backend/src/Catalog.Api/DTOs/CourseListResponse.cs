namespace Catalog.Api.DTOs;

public sealed record CourseListResponse(IReadOnlyList<CourseSummaryDto> Items, int TotalCount);
