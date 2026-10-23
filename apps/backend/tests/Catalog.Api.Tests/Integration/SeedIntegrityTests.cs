using System.Text.Json;
using Catalog.Api.Data;
using Catalog.Api.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class SeedIntegrityTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task IndependentEmptyDatabasesReceiveIdenticalData()
    {
        await using var first = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await using var second = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        Assert.Equal(await SnapshotAsync(first), await SnapshotAsync(second));

        var before = await SnapshotAsync(first);
        await first.Database.MigrateAsync();
        Assert.Equal(before, await SnapshotAsync(first));
    }

    [Fact]
    public async Task PersistsEverySeedAttributeAndEducationalRelationship()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        var courses = await context.Courses.AsNoTracking().Include(course => course.Category)
            .Include(course => course.Modality).Include(course => course.Status)
            .Include(course => course.Modules).ThenInclude(module => module.Lessons)
            .ToListAsync();
        var expected = CatalogSeedData.CreateCourses().ToDictionary(course => course.Id);

        Assert.Equal(6, courses.Count);
        Assert.Equal(3, await context.Categories.CountAsync());
        Assert.Equal(2, await context.Modalities.CountAsync());
        Assert.Equal(3, await context.CourseStatuses.CountAsync());
        Assert.Contains(courses, course => course.AvailableOn is null);
        Assert.Contains(courses, course => course.AvailableOn is not null);
        Assert.True(courses.Select(course => course.WorkloadHours).Distinct().Count() > 1);
        Assert.True(courses.Select(course => course.TargetAudience).Distinct().Count() > 1);
        Assert.True(courses.Select(course => course.Modules.Count).Distinct().Count() > 1);

        foreach (var course in courses)
        {
            var definition = expected[course.Id];
            Assert.Equal(definition.FullName, course.FullName);
            Assert.Equal(definition.ShortName, course.ShortName);
            Assert.Equal(definition.Description, course.Description);
            Assert.Equal(definition.WorkloadHours, course.WorkloadHours);
            Assert.Equal(definition.TargetAudience, course.TargetAudience);
            Assert.Equal(definition.ImageUrl, course.ImageUrl);
            Assert.Equal(definition.AvailableOn, course.AvailableOn);
            Assert.Equal(definition.CategoryId, course.Category.Id);
            Assert.Equal(definition.ModalityId, course.Modality.Id);
            Assert.Equal(definition.CourseStatusId, course.Status.Id);
            Assert.Equal(CatalogSeedData.CreateCategories().Single(item => item.Id == course.CategoryId).Name, course.Category.Name);
            Assert.Equal(CatalogSeedData.CreateModalities().Single(item => item.Id == course.ModalityId).Name, course.Modality.Name);
            Assert.Equal(CatalogSeedData.CreateStatuses().Single(item => item.Id == course.CourseStatusId).Name, course.Status.Name);
            Assert.Equal(definition.Modules.Count, course.Modules.Count);

            foreach (var module in course.Modules)
            {
                var expectedModule = definition.Modules.Single(item => item.Id == module.Id);
                Assert.Equal(course.Id, module.CourseId);
                Assert.Equal(expectedModule.Title, module.Title);
                Assert.Equal(expectedModule.Description, module.Description);
                Assert.Equal(expectedModule.DisplayOrder, module.DisplayOrder);
                Assert.Equal(expectedModule.Lessons.Count, module.Lessons.Count);
                foreach (var lesson in module.Lessons)
                {
                    var expectedLesson = expectedModule.Lessons.Single(item => item.Id == lesson.Id);
                    Assert.Equal(module.Id, lesson.CourseModuleId);
                    Assert.Equal(expectedLesson.Title, lesson.Title);
                    Assert.Equal(expectedLesson.Description, lesson.Description);
                    Assert.Equal(expectedLesson.DisplayOrder, lesson.DisplayOrder);
                }
            }
        }
        var descriptions = courses.SelectMany(course => course.Modules).SelectMany(module => module.Lessons)
            .Select(lesson => lesson.Description).Distinct();
        Assert.True(descriptions.Count() > 1);
    }

    [Fact]
    public async Task CompletesMissingModulesAndLessonsWithoutDuplicatingOtherRows()
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        string original;
        await using (var context = PostgreSqlFixture.CreateContext(connection))
        {
            original = await SnapshotAsync(context);
            await context.CourseModules.Where(module => module.Id == 201).ExecuteDeleteAsync();
            await context.Lessons.Where(lesson => lesson.Id == 10101).ExecuteDeleteAsync();
        }

        await using var initialized = PostgreSqlFixture.CreateContext(connection);
        await initialized.Database.MigrateAsync();
        Assert.Equal(original, await SnapshotAsync(initialized));
    }

    private static async Task<string> SnapshotAsync(CatalogDbContext context) => JsonSerializer.Serialize(new
    {
        Categories = await context.Categories.AsNoTracking().OrderBy(item => item.Id).ToArrayAsync(),
        Modalities = await context.Modalities.AsNoTracking().OrderBy(item => item.Id).ToArrayAsync(),
        Statuses = await context.CourseStatuses.AsNoTracking().OrderBy(item => item.Id).ToArrayAsync(),
        Courses = await context.Courses.AsNoTracking().OrderBy(item => item.Id).ToArrayAsync(),
        Modules = await context.CourseModules.AsNoTracking().OrderBy(item => item.Id).ToArrayAsync(),
        Lessons = await context.Lessons.AsNoTracking().OrderBy(item => item.Id).ToArrayAsync()
    });
}
