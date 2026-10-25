using Catalog.Api.DTOs;
using Catalog.Api.Options;
using Catalog.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class QueryServiceTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task ListsAllStatusesAndDatesWithoutTrackingEntities()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        var result = await CreateService(context).ListAsync(new());
        Assert.Equal(6, result.TotalCount);
        Assert.Equal(result.Items.Count, result.TotalCount);
        Assert.Equal(new[] { 1, 2, 3 }, result.Items.Select(course => course.Status.Id).Distinct().Order().ToArray());
        Assert.Contains(result.Items, course => course.Id == 3 && course.AvailableOn == new DateOnly(2027, 2, 10));
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task CombinesLiteralNameSearchWithAllClassificationFilters()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        var service = CreateService(context);
        var result = await service.ListAsync(new CourseQueryParameters
        {
            Search = "  gEoTêXtiL  ", CategoryId = 1, ModalityId = 2, StatusId = 1
        });
        Assert.Equal(2, Assert.Single(result.Items).Id);
        Assert.Equal(1, result.TotalCount);

        var empty = await service.ListAsync(new CourseQueryParameters
        {
            Search = "GeoTêxtil", CategoryId = 1, ModalityId = 1, StatusId = 2
        });
        Assert.Empty(empty.Items);
        Assert.Equal(0, empty.TotalCount);
    }

    [Theory]
    [InlineData("%")]
    [InlineData("_")]
    [InlineData("\\")]
    public async Task DoesNotTreatUserCharactersAsSqlWildcards(string search)
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        var result = await CreateService(context).ListAsync(new() { Search = search });
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task ProjectsCompleteDetailsAndOrdersModulesAndLessonsWithIdTieBreaks()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        var modules = await context.CourseModules.Where(module => module.CourseId == 3).ToListAsync();
        foreach (var module in modules)
        {
            module.DisplayOrder = module.Id == 303 ? 1 : 2;
        }
        var lessons = await context.Lessons.Where(lesson => lesson.CourseModuleId == 301).ToListAsync();
        foreach (var lesson in lessons)
        {
            lesson.DisplayOrder = lesson.Id == 30102 ? 1 : 2;
        }
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var detail = await CreateService(context).GetDetailsAsync(3);
        Assert.NotNull(detail);
        Assert.Equal("Estatística e Controle de Qualidade na Produção", detail.FullName);
        Assert.Equal("Matemática", detail.Category.Name);
        Assert.Equal(24, detail.WorkloadHours);
        Assert.Equal(new[] { 303, 301, 302 }, detail.Modules.Select(module => module.Id).ToArray());
        Assert.Equal(new[] { 30102, 30101 }, detail.Modules.Single(module => module.Id == 301)
            .Lessons.Select(lesson => lesson.Id).ToArray());
        Assert.All(detail.Modules, module => Assert.False(string.IsNullOrWhiteSpace(module.Description)));
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task SupportsEmptyEducationalCollectionsAndMissingCourse()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await context.CourseModules.Where(module => module.CourseId == 1).ExecuteDeleteAsync();
        await context.Lessons.Where(lesson => lesson.Module.CourseId == 2).ExecuteDeleteAsync();
        var service = CreateService(context);

        var withoutModules = await service.GetDetailsAsync(1);
        Assert.NotNull(withoutModules);
        Assert.Empty(withoutModules.Modules);
        var withoutLessons = await service.GetDetailsAsync(2);
        Assert.NotNull(withoutLessons);
        Assert.All(withoutLessons.Modules, module => Assert.Empty(module.Lessons));
        Assert.Null(await service.GetDetailsAsync(999999));
    }

    private static CourseQueryService CreateService(Catalog.Api.Data.CatalogDbContext context)
    {
        var options = Microsoft.Extensions.Options.Options.Create(
            new FrontendOriginOptions { Value = "http://127.0.0.1:5173" });
        return new CourseQueryService(context, new CourseImageUrlResolver(options));
    }
}
