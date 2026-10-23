using System.Net;
using System.Net.Http.Json;
using Catalog.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CourseFilterHttpTests(PostgreSqlFixture postgres)
{
    [Theory]
    [InlineData("categoryId=1", "1,2")]
    [InlineData("categoryId=2", "3,4")]
    [InlineData("categoryId=3", "5,6")]
    [InlineData("modalityId=1", "1,3,5")]
    [InlineData("modalityId=2", "2,4,6")]
    [InlineData("statusId=1", "1,3")]
    [InlineData("statusId=2", "2,5")]
    [InlineData("statusId=3", "4,6")]
    [InlineData("categoryId=1&modalityId=1", "1")]
    [InlineData("categoryId=1&modalityId=2", "2")]
    [InlineData("categoryId=2&statusId=3", "4")]
    [InlineData("modalityId=1&statusId=2", "5")]
    [InlineData("categoryId=3&modalityId=2&statusId=3", "6")]
    [InlineData("search=Educa&categoryId=1", "1")]
    [InlineData("search=TecEdu&categoryId=1&modalityId=1&statusId=1", "1")]
    [InlineData("search=Digital&categoryId=1&modalityId=2&statusId=2", "2")]
    [InlineData("search=Educa&categoryId=2&modalityId=1&statusId=1", "3")]
    [InlineData("categoryId=999999", "")]
    [InlineData("modalityId=999999", "")]
    [InlineData("statusId=999999", "")]
    [InlineData("categoryId=-1", "")]
    [InlineData("categoryId=1&modalityId=1&statusId=3", "")]
    [InlineData("search=DigitalLab&categoryId=1&modalityId=1&statusId=2", "")]
    [InlineData("search=termo_sem_correspondencia&categoryId=1&modalityId=1&statusId=1", "")]
    public async Task AppliesEveryCriterionCumulativelyAndCountsOnlyMatchingRows(string query, string expected)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync($"/api/courses?{query}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        var expectedIds = expected.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        Assert.Equal(expectedIds, result.Items.Select(course => course.Id).Order().ToArray());
        Assert.Equal(expectedIds.Length, result.TotalCount);
        Assert.Equal(result.Items.Count, result.TotalCount);
    }

    [Fact]
    public async Task EmptyCatalogReturnsEmptyArrayAndZeroCount()
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        await using (var context = PostgreSqlFixture.CreateContext(connection))
        {
            await context.Courses.ExecuteDeleteAsync();
        }
        await using var factory = new CatalogApiFactory(connection);
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/courses");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }
}
