using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Configurations;

public sealed class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons");
        builder.HasKey(lesson => lesson.Id);
        builder.Property(lesson => lesson.Id).ValueGeneratedNever();
        builder.Property(lesson => lesson.Title).IsRequired().HasMaxLength(200);
        builder.Property(lesson => lesson.Description).IsRequired().HasMaxLength(4000);
        builder.Property(lesson => lesson.DisplayOrder).IsRequired();
        builder.HasOne(lesson => lesson.Module).WithMany(module => module.Lessons)
            .HasForeignKey(lesson => lesson.CourseModuleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(lesson => new { lesson.CourseModuleId, lesson.DisplayOrder });
    }
}
