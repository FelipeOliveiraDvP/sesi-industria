using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data.Seed;

public static class CatalogSeed
{
    public static void Apply(CatalogDbContext context)
    {
        var (courses, modules, lessons) = CreateCourseRows();
        AddMissing(context, CatalogSeedData.CreateCategories(), entity => entity.Id);
        AddMissing(context, CatalogSeedData.CreateModalities(), entity => entity.Id);
        AddMissing(context, CatalogSeedData.CreateStatuses(), entity => entity.Id);
        AddMissing(context, courses, entity => entity.Id);
        AddMissing(context, modules, entity => entity.Id);
        AddMissing(context, lessons, entity => entity.Id);
        context.SaveChanges();
    }

    public static async Task ApplyAsync(CatalogDbContext context, CancellationToken cancellationToken)
    {
        var (courses, modules, lessons) = CreateCourseRows();
        await AddMissingAsync(context, CatalogSeedData.CreateCategories(), entity => entity.Id, cancellationToken);
        await AddMissingAsync(context, CatalogSeedData.CreateModalities(), entity => entity.Id, cancellationToken);
        await AddMissingAsync(context, CatalogSeedData.CreateStatuses(), entity => entity.Id, cancellationToken);
        await AddMissingAsync(context, courses, entity => entity.Id, cancellationToken);
        await AddMissingAsync(context, modules, entity => entity.Id, cancellationToken);
        await AddMissingAsync(context, lessons, entity => entity.Id, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static void AddMissing<TEntity>(CatalogDbContext context, IEnumerable<TEntity> data,
        Func<TEntity, int> id) where TEntity : class
    {
        var existing = context.Set<TEntity>().Select(entity => EF.Property<int>(entity, "Id")).ToHashSet();
        context.Set<TEntity>().AddRange(data.Where(entity => !existing.Contains(id(entity))));
    }

    private static async Task AddMissingAsync<TEntity>(CatalogDbContext context, IEnumerable<TEntity> data,
        Func<TEntity, int> id, CancellationToken cancellationToken) where TEntity : class
    {
        var existing = await context.Set<TEntity>().Select(entity => EF.Property<int>(entity, "Id"))
            .ToHashSetAsync(cancellationToken);
        context.Set<TEntity>().AddRange(data.Where(entity => !existing.Contains(id(entity))));
    }

    private static (Course[] Courses, CourseModule[] Modules, Lesson[] Lessons) CreateCourseRows()
    {
        var courses = CatalogSeedData.CreateCourses();
        var modules = courses.SelectMany(course => course.Modules).ToArray();
        var lessons = modules.SelectMany(module => module.Lessons).ToArray();

        // Insert each table independently so a partially initialized graph can be completed.
        foreach (var course in courses)
        {
            course.Modules = [];
        }
        foreach (var module in modules)
        {
            module.Lessons = [];
        }
        return (courses, modules, lessons);
    }
}
