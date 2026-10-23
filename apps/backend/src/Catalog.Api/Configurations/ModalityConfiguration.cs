using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.Configurations;

public sealed class ModalityConfiguration : IEntityTypeConfiguration<Modality>
{
    public void Configure(EntityTypeBuilder<Modality> builder)
    {
        builder.ToTable("Modalities");
        builder.HasKey(modality => modality.Id);
        builder.Property(modality => modality.Id).ValueGeneratedNever();
        builder.Property(modality => modality.Name).IsRequired().HasMaxLength(120);
    }
}
