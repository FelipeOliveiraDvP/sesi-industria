namespace Catalog.Api.Entities;

public sealed class Modality
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Course> Courses { get; set; } = [];
}
