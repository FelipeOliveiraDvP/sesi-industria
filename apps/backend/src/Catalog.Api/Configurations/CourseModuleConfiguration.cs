using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Configurations;

public sealed class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.ToTable("CourseModules");
        builder.HasKey(module => module.Id);
        builder.Property(module => module.Id).ValueGeneratedNever();
        builder.Property(module => module.Title).IsRequired().HasMaxLength(200);
        builder.Property(module => module.Description).IsRequired().HasMaxLength(4000);
        builder.Property(module => module.DisplayOrder).IsRequired();
        builder.HasOne(module => module.Course).WithMany(course => course.Modules)
            .HasForeignKey(module => module.CourseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(module => new { module.CourseId, module.DisplayOrder });
    }
}
