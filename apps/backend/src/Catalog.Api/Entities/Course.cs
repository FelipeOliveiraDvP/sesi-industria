namespace Catalog.Api.Entities;

public sealed class Course
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string ShortName { get; set; }
    public required string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int ModalityId { get; set; }
    public Modality Modality { get; set; } = null!;
    public int CourseStatusId { get; set; }
    public CourseStatus Status { get; set; } = null!;
    public decimal WorkloadHours { get; set; }
    public required string TargetAudience { get; set; }
    public required string ImageUrl { get; set; }
    public DateOnly? AvailableOn { get; set; }
    public ICollection<CourseModule> Modules { get; set; } = [];
}
