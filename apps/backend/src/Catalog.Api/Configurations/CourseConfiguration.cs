using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Configurations;

public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(course => course.Id);
        builder.Property(course => course.Id).ValueGeneratedNever();
        builder.Property(course => course.FullName).IsRequired().HasMaxLength(200);
        builder.Property(course => course.ShortName).IsRequired().HasMaxLength(80);
        builder.Property(course => course.Description).IsRequired().HasMaxLength(4000);
        builder.Property(course => course.ShortDescription).IsRequired().HasMaxLength(1000);
        builder.Property(course => course.PresentationJson).IsRequired().HasColumnType("jsonb");
        builder.Property(course => course.TargetAudience).IsRequired().HasMaxLength(1000);
        builder.Property(course => course.ImageUrl).IsRequired().HasMaxLength(512);
        builder.Property(course => course.WorkloadHours).IsRequired().HasPrecision(7, 2);
        builder.Property(course => course.AvailableOn).HasColumnType("date").IsRequired(false);

        builder.HasOne(course => course.Category).WithMany(category => category.Courses)
            .HasForeignKey(course => course.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(course => course.Modality).WithMany(modality => modality.Courses)
            .HasForeignKey(course => course.ModalityId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(course => course.Status).WithMany(status => status.Courses)
            .HasForeignKey(course => course.CourseStatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(course => course.CategoryId);
        builder.HasIndex(course => course.ModalityId);
        builder.HasIndex(course => course.CourseStatusId);
    }
}
