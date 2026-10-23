namespace Catalog.Api.Entities;

public sealed class CourseModule
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int DisplayOrder { get; set; }
    public ICollection<Lesson> Lessons { get; set; } = [];
}
