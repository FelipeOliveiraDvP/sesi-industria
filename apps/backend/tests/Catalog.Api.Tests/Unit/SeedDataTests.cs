using System.Text.Json;
using Catalog.Api.Data.Seed;
using Xunit;

namespace Catalog.Api.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class SeedDataTests
{
    [Fact]
    public void DefinesMinimumFictionalCatalogAndValidClassifications()
    {
        var courses = CatalogSeedData.CreateCourses();
        var categories = CatalogSeedData.CreateCategories();
        var modalities = CatalogSeedData.CreateModalities();
        var statuses = CatalogSeedData.CreateStatuses();
        Assert.True(courses.Length >= 6);
        Assert.True(categories.Select(category => category.Name).Distinct().Count() >= 3);
        Assert.True(modalities.Select(modality => modality.Name).Distinct().Count() >= 2);
        Assert.True(statuses.Select(status => status.Name).Distinct().Count() >= 3);
        Assert.Equal(courses.Length, courses.Select(course => course.Id).Distinct().Count());

        Assert.All(courses, course =>
        {
            Assert.All(new[] { course.FullName, course.ShortName, course.Description, course.TargetAudience, course.ImageUrl },
                value => Assert.False(string.IsNullOrWhiteSpace(value)));
            Assert.Contains(categories, category => category.Id == course.CategoryId);
            Assert.Contains(modalities, modality => modality.Id == course.ModalityId);
            Assert.Contains(statuses, status => status.Id == course.CourseStatusId);
            Assert.StartsWith("/images/courses/", course.ImageUrl);
        });
    }

    [Fact]
    public void VariesWorkloadsAudiencesModulesLessonsAndAvailability()
    {
        var courses = CatalogSeedData.CreateCourses();
        Assert.True(courses.Select(course => course.WorkloadHours).Distinct().Count() > 1);
        Assert.True(courses.Select(course => course.TargetAudience).Distinct().Count() > 1);
        Assert.True(courses.Select(course => course.Modules.Count).Distinct().Count() > 1);
        Assert.Contains(courses, course => course.AvailableOn is null);
        Assert.Contains(courses, course => course.AvailableOn is not null);

        var modules = courses.SelectMany(course => course.Modules).ToArray();
        var lessons = modules.SelectMany(module => module.Lessons).ToArray();
        Assert.Equal(modules.Length, modules.Select(module => module.Id).Distinct().Count());
        Assert.Equal(lessons.Length, lessons.Select(lesson => lesson.Id).Distinct().Count());
        Assert.True(lessons.Select(lesson => lesson.Description).Distinct().Count() > 1);
        Assert.All(courses, course => Assert.All(course.Modules, module =>
        {
            Assert.Equal(course.Id, module.CourseId);
            Assert.False(string.IsNullOrWhiteSpace(module.Description));
            Assert.All(module.Lessons, lesson =>
            {
                Assert.Equal(module.Id, lesson.CourseModuleId);
                Assert.False(string.IsNullOrWhiteSpace(lesson.Title));
                Assert.False(string.IsNullOrWhiteSpace(lesson.Description));
            });
        }));
    }

    [Fact]
    public void CreatesFreshGraphsWithIdenticalValuesOnEveryCall()
    {
        var first = CatalogSeedData.CreateCourses();
        var second = CatalogSeedData.CreateCourses();
        Assert.Equal(JsonSerializer.Serialize(first), JsonSerializer.Serialize(second));
        Assert.Equal(JsonSerializer.Serialize(CatalogSeedData.CreateCategories()), JsonSerializer.Serialize(CatalogSeedData.CreateCategories()));
        Assert.Equal(JsonSerializer.Serialize(CatalogSeedData.CreateModalities()), JsonSerializer.Serialize(CatalogSeedData.CreateModalities()));
        Assert.Equal(JsonSerializer.Serialize(CatalogSeedData.CreateStatuses()), JsonSerializer.Serialize(CatalogSeedData.CreateStatuses()));
        Assert.NotSame(first[0], second[0]);
        Assert.NotSame(first[0].Modules.First(), second[0].Modules.First());
    }
}
