namespace Catalog.Api.DTOs;

public sealed record CourseModuleDto(
    int Id, string Title, string Description, int Order, IReadOnlyList<LessonDto> Lessons);
