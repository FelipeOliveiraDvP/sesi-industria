using System.Text.Json;
using System.Text.Json.Nodes;
using Catalog.Api.Content;
using Catalog.Api.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class AvaPresentationMigrationTests(PostgreSqlFixture postgres)
{
    private const string PreviousMigration = "20261023202554_AddCoursePresentationFields";

    [Fact]
    public async Task AddsGenericAvaLessonsToExistingPresentationsAndPreservesOtherData()
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        await using var context = PostgreSqlFixture.CreateContext(connection);

        var originalPresentations = await context.Courses.AsNoTracking()
            .ToDictionaryAsync(course => course.Id, course => course.PresentationJson);
        var originalModules = JsonSerializer.Serialize(await context.CourseModules.AsNoTracking()
            .OrderBy(module => module.Id)
            .Select(module => new { module.Id, module.CourseId, module.Title, module.Description, module.DisplayOrder })
            .ToListAsync());
        var originalLessons = JsonSerializer.Serialize(await context.Lessons.AsNoTracking()
            .OrderBy(lesson => lesson.Id)
            .Select(lesson => new { lesson.Id, lesson.CourseModuleId, lesson.Title, lesson.Description, lesson.DisplayOrder })
            .ToListAsync());

        await context.Database.MigrateAsync(PreviousMigration);

        var legacyPresentations = await context.Courses.AsNoTracking().ToListAsync();
        Assert.Equal(6, legacyPresentations.Count);
        foreach (var course in legacyPresentations)
        {
            using var document = JsonDocument.Parse(course.PresentationJson);
            var modules = document.RootElement.GetProperty("learningPreview").GetProperty("modules");
            Assert.Equal(6, modules.GetArrayLength());
            Assert.All(modules.EnumerateArray(), module => Assert.False(module.TryGetProperty("lessons", out _)));
        }

        await context.Database.MigrateAsync();
        var migratedCourses = await context.Courses.AsNoTracking().OrderBy(course => course.Id).ToListAsync();
        Assert.Equal(6, migratedCourses.Count);

        string? expectedOutline = null;
        foreach (var course in migratedCourses)
        {
            var presentation = CoursePresentationSerializer.Deserialize(course.PresentationJson);
            Assert.Equal(6, presentation.LearningPreview.Modules.Count);
            Assert.All(presentation.LearningPreview.Modules, module => Assert.Equal(2, module.Lessons.Count));
            var outline = JsonSerializer.Serialize(presentation.LearningPreview.Modules);
            expectedOutline ??= outline;
            Assert.Equal(expectedOutline, outline);
            Assert.Equal(WithoutAvaModules(originalPresentations[course.Id]), WithoutAvaModules(course.PresentationJson));
        }

        Assert.Equal(originalModules, JsonSerializer.Serialize(await context.CourseModules.AsNoTracking()
            .OrderBy(module => module.Id)
            .Select(module => new { module.Id, module.CourseId, module.Title, module.Description, module.DisplayOrder })
            .ToListAsync()));
        Assert.Equal(originalLessons, JsonSerializer.Serialize(await context.Lessons.AsNoTracking()
            .OrderBy(lesson => lesson.Id)
            .Select(lesson => new { lesson.Id, lesson.CourseModuleId, lesson.Title, lesson.Description, lesson.DisplayOrder })
            .ToListAsync()));

        await context.Database.MigrateAsync();
        var repeated = await context.Courses.AsNoTracking().OrderBy(course => course.Id).ToListAsync();
        Assert.Equal(6, repeated.Count);
        Assert.All(repeated, course =>
            Assert.Equal(expectedOutline, JsonSerializer.Serialize(
                CoursePresentationSerializer.Deserialize(course.PresentationJson).LearningPreview.Modules)));
    }

    private static string WithoutAvaModules(string presentationJson)
    {
        var root = JsonNode.Parse(presentationJson)!.AsObject();
        root["learningPreview"]!.AsObject().Remove("modules");
        return root.ToJsonString();
    }
}
