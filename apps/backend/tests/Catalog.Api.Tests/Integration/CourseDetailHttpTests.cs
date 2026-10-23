using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Catalog.Api.Data.Seed;
using Catalog.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CourseDetailHttpTests(PostgreSqlFixture postgres)
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public async Task ReturnsAllFieldsClassificationsModulesAndLessons(int id)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync($"/api/courses/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var expectedFields = new[] { "id", "fullName", "shortName", "description", "category", "modality",
            "status", "workloadHours", "targetAudience", "imageUrl", "availableOn", "modules" };
        Assert.Equal(expectedFields.Order().ToArray(), json.EnumerateObject().Select(property => property.Name).Order().ToArray());
        var actual = json.Deserialize<CourseDetailDto>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.NotNull(actual);
        var expected = CatalogSeedData.CreateCourses().Single(course => course.Id == id);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.FullName, actual.FullName);
        Assert.Equal(expected.ShortName, actual.ShortName);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.WorkloadHours, actual.WorkloadHours);
        Assert.Equal(expected.TargetAudience, actual.TargetAudience);
        Assert.Equal(expected.ImageUrl, actual.ImageUrl);
        Assert.Equal(expected.AvailableOn, actual.AvailableOn);
        if (expected.AvailableOn is null)
        {
            Assert.Equal(JsonValueKind.Null, json.GetProperty("availableOn").ValueKind);
        }
        Assert.Equal(expected.CategoryId, actual.Category.Id);
        Assert.Equal(expected.ModalityId, actual.Modality.Id);
        Assert.Equal(expected.CourseStatusId, actual.Status.Id);
        Assert.Equal(CatalogSeedData.CreateCategories().Single(item => item.Id == expected.CategoryId).Name, actual.Category.Name);
        Assert.Equal(CatalogSeedData.CreateModalities().Single(item => item.Id == expected.ModalityId).Name, actual.Modality.Name);
        Assert.Equal(CatalogSeedData.CreateStatuses().Single(item => item.Id == expected.CourseStatusId).Name, actual.Status.Name);
        Assert.Equal(expected.Modules.Select(module => module.Id).ToArray(), actual.Modules.Select(module => module.Id).ToArray());
        foreach (var module in actual.Modules)
        {
            var definition = expected.Modules.Single(item => item.Id == module.Id);
            Assert.Equal(definition.Title, module.Title);
            Assert.Equal(definition.Description, module.Description);
            Assert.Equal(definition.DisplayOrder, module.Order);
            Assert.Equal(definition.Lessons.Select(lesson => lesson.Id).ToArray(), module.Lessons.Select(lesson => lesson.Id).ToArray());
            foreach (var lesson in module.Lessons)
            {
                var expectedLesson = definition.Lessons.Single(item => item.Id == lesson.Id);
                Assert.Equal(expectedLesson.Title, lesson.Title);
                Assert.Equal(expectedLesson.Description, lesson.Description);
                Assert.Equal(expectedLesson.DisplayOrder, lesson.Order);
            }
        }
    }

    [Fact]
    public async Task UsesPresentationOrderThenIdRatherThanInsertionOrder()
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        await using (var context = PostgreSqlFixture.CreateContext(connection))
        {
            foreach (var module in await context.CourseModules.Where(module => module.CourseId == 3).ToListAsync())
            {
                module.DisplayOrder = module.Id == 303 ? 1 : 2;
            }
            foreach (var lesson in await context.Lessons.Where(lesson => lesson.CourseModuleId == 301).ToListAsync())
            {
                lesson.DisplayOrder = lesson.Id == 30102 ? 1 : 2;
            }
            foreach (var lesson in await context.Lessons.Where(lesson => lesson.CourseModuleId == 303).ToListAsync())
            {
                lesson.DisplayOrder = 1;
            }
            await context.SaveChangesAsync();
        }
        await using var factory = new CatalogApiFactory(connection);
        using var client = factory.CreateClient();
        var detail = await client.GetFromJsonAsync<CourseDetailDto>("/api/courses/3");
        Assert.NotNull(detail);
        Assert.Equal(new[] { 303, 301, 302 }, detail.Modules.Select(module => module.Id).ToArray());
        Assert.Equal(new[] { 30102, 30101 }, detail.Modules.Single(module => module.Id == 301).Lessons.Select(lesson => lesson.Id).ToArray());
        Assert.Equal(new[] { 30301, 30302 }, detail.Modules.Single(module => module.Id == 303).Lessons.Select(lesson => lesson.Id).ToArray());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ReturnsEmptyCollectionsWhenEducationalContentIsAbsent(bool removeModules)
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        await using (var context = PostgreSqlFixture.CreateContext(connection))
        {
            if (removeModules)
            {
                await context.CourseModules.Where(module => module.CourseId == 1).ExecuteDeleteAsync();
            }
            else
            {
                await context.Lessons.Where(lesson => lesson.Module.CourseId == 1).ExecuteDeleteAsync();
            }
        }
        await using var factory = new CatalogApiFactory(connection);
        using var client = factory.CreateClient();
        var detail = await client.GetFromJsonAsync<CourseDetailDto>("/api/courses/1");
        Assert.NotNull(detail);
        if (removeModules)
        {
            Assert.Empty(detail.Modules);
        }
        else
        {
            Assert.NotEmpty(detail.Modules);
            Assert.All(detail.Modules, module => Assert.Empty(module.Lessons));
        }
    }

    [Fact]
    public async Task MissingCourseReturns404WithProblemDetails()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/courses/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
    }
}
