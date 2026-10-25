using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Catalog.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CourseSearchHttpTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task ListsAllCoursesWithClassificationNamesAndCorrectEnvelope()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/courses");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType!.MediaType);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var items = json.GetProperty("items");
        Assert.Equal(6, json.GetProperty("totalCount").GetInt32());
        Assert.Equal(6, items.GetArrayLength());
        foreach (var item in items.EnumerateArray())
        {
            Assert.True(item.GetProperty("id").GetInt32() > 0);
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("fullName").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("shortName").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("description").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("shortDescription").GetString()));
            Assert.StartsWith("http://127.0.0.1:5173/assets/", item.GetProperty("imageUrl").GetString());
            foreach (var classification in new[] { "category", "modality", "status" })
            {
                Assert.True(item.GetProperty(classification).GetProperty("id").GetInt32() > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.GetProperty(classification).GetProperty("name").GetString()));
            }
            Assert.False(item.TryGetProperty("categoryId", out _));
        }
    }

    [Theory]
    [InlineData("Matemática para a Precisão: Medidas e Escalas na Metalurgia", 1)]
    [InlineData("Física Industrial", 4)]
    [InlineData("Precisão", 1)]
    [InlineData("  mEtRoLoGiA  ", 1)]
    [InlineData("GeoTêxtil", 2)]
    public async Task SearchesFullNameOrShortNameAsCaseInsensitiveSubstring(string search, int expectedId)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        var result = await client.GetFromJsonAsync<CourseListResponse>($"/api/courses?search={Uri.EscapeDataString(search)}");
        Assert.NotNull(result);
        Assert.Equal(expectedId, Assert.Single(result.Items).Id);
        Assert.Equal(1, result.TotalCount);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public async Task BlankSearchDoesNotRestrictResults(string search)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        var result = await client.GetFromJsonAsync<CourseListResponse>($"/api/courses?search={Uri.EscapeDataString(search)}");
        Assert.NotNull(result);
        Assert.Equal(6, result.TotalCount);
        Assert.Equal(6, result.Items.Count);
    }

    [Theory]
    [InlineData("%")]
    [InlineData("_")]
    [InlineData("\\")]
    [InlineData("curso-inexistente-xyz")]
    public async Task ReturnsEmptyCollectionWhenLiteralTermDoesNotOccur(string search)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync($"/api/courses?search={Uri.EscapeDataString(search)}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task SearchDoesNotMatchCourseDescriptions()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();

        var result = await client.GetFromJsonAsync<CourseListResponse>("/api/courses?search=fura%C3%A7%C3%A3o");

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task FindsActualPercentUnderscoreAndBackslashWithoutWildcardExpansion()
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        await using (var context = PostgreSqlFixture.CreateContext(connection))
        {
            var course = await context.Courses.SingleAsync(course => course.Id == 1);
            course.ShortName = @"Edu_100%\Lab";
            await context.SaveChangesAsync();
        }
        await using var factory = new CatalogApiFactory(connection);
        using var client = factory.CreateClient();
        foreach (var search in new[] { "%", "_", "\\", @"Edu_100%\Lab" })
        {
            var result = await client.GetFromJsonAsync<CourseListResponse>($"/api/courses?search={Uri.EscapeDataString(search)}");
            Assert.NotNull(result);
            Assert.Equal(1, Assert.Single(result.Items).Id);
            Assert.Equal(1, result.TotalCount);
        }
    }
}
