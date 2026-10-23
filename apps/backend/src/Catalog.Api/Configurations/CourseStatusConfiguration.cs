using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Configurations;

public sealed class CourseStatusConfiguration : IEntityTypeConfiguration<CourseStatus>
{
    public void Configure(EntityTypeBuilder<CourseStatus> builder)
    {
        builder.ToTable("CourseStatuses");
        builder.HasKey(status => status.Id);
        builder.Property(status => status.Id).ValueGeneratedNever();
        builder.Property(status => status.Name).IsRequired().HasMaxLength(120);
    }
}
