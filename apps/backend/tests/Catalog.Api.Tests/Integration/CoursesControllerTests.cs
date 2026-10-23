using System.Net;
using System.Net.Http.Json;
using Catalog.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CoursesControllerTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task ListsAndRetrievesCourseThroughHttp()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        var list = await client.GetFromJsonAsync<CourseListResponse>("/api/courses");
        Assert.NotNull(list);
        Assert.Equal(6, list.TotalCount);
        var detail = await client.GetFromJsonAsync<CourseDetailDto>("/api/courses/1");
        Assert.NotNull(detail);
        Assert.Equal("TecEdu", detail.ShortName);
        Assert.NotEmpty(detail.Modules);
    }

    [Theory]
    [InlineData("/api/courses?categoryId=abc")]
    [InlineData("/api/courses?modalityId=abc")]
    [InlineData("/api/courses?statusId=abc")]
    [InlineData("/api/courses?categoryId=2147483648")]
    [InlineData("/api/courses/abc")]
    public async Task RejectsMalformedIdentifiers(string path)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.NotEmpty(problem.Errors);
    }

    [Fact]
    public async Task ReportsMissingCourseAsProblemDetails()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/courses/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(404, (await response.Content.ReadFromJsonAsync<ProblemDetails>())!.Status);
    }

    [Fact]
    public async Task DoesNotExposeCourseCreation()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.PostAsJsonAsync("/api/courses", new { fullName = "Não deve ser criado" });
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        Assert.Equal(6, (await client.GetFromJsonAsync<CourseListResponse>("/api/courses"))!.TotalCount);
    }
}
