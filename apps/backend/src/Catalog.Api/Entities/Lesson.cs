namespace Catalog.Api.Entities;

public sealed class Lesson
{
    public int Id { get; set; }
    public int CourseModuleId { get; set; }
    public CourseModule Module { get; set; } = null!;
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int DisplayOrder { get; set; }
}
