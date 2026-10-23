using Catalog.Api.Entities;
using Catalog.Api.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Modality> Modalities => Set<Modality>();
    public DbSet<CourseStatus> CourseStatuses => Set<CourseStatus>();
    public DbSet<CourseModule> CourseModules => Set<CourseModule>();
    public DbSet<Lesson> Lessons => Set<Lesson>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) => CatalogSeed.Apply((CatalogDbContext)context))
            .UseAsyncSeeding((context, _, cancellationToken) =>
                CatalogSeed.ApplyAsync((CatalogDbContext)context, cancellationToken));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
