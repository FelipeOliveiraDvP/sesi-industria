using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class OpenApiTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task DocumentsExactlyTwoReadOnlyRoutesAndAllOptionalFilters()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        var document = await client.GetFromJsonAsync<JsonElement>("/openapi/v1.json");
        Assert.StartsWith("3.", document.GetProperty("openapi").GetString()!);
        var paths = document.GetProperty("paths");
        Assert.Equal(new[] { "/api/courses", "/api/courses/{id}" }, paths.EnumerateObject()
            .Select(property => property.Name).Order().ToArray());
        foreach (var path in paths.EnumerateObject())
        {
            Assert.True(path.Value.TryGetProperty("get", out _));
            foreach (var method in new[] { "post", "put", "patch", "delete" })
            {
                Assert.False(path.Value.TryGetProperty(method, out _));
            }
        }

        var list = paths.GetProperty("/api/courses").GetProperty("get");
        var parameters = list.GetProperty("parameters").EnumerateArray().ToArray();
        Assert.Equal(new[] { "categoryId", "modalityId", "search", "statusId" }, parameters
            .Select(parameter => parameter.GetProperty("name").GetString()).Order().ToArray());
        Assert.All(parameters, parameter =>
        {
            Assert.Equal("query", parameter.GetProperty("in").GetString());
            Assert.False(parameter.TryGetProperty("required", out var required) && required.GetBoolean());
        });
        Assert.True(list.GetProperty("responses").TryGetProperty("200", out _));
        Assert.True(list.GetProperty("responses").TryGetProperty("400", out _));
        Assert.True(list.GetProperty("responses").TryGetProperty("500", out _));
        Assert.True(paths.GetProperty("/api/courses/{id}").GetProperty("get").GetProperty("responses")
            .TryGetProperty("404", out _));

        var schemas = document.GetProperty("components").GetProperty("schemas");
        foreach (var schema in new[] { "CourseListResponse", "CourseSummaryDto", "CourseDetailDto", "CourseModuleDto", "LessonDto", "LookupDto" })
        {
            Assert.True(schemas.TryGetProperty(schema, out _), schema);
        }
    }

    [Fact]
    public async Task ServesSwaggerUiAndItsEmbeddedAssetsLocally()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        var html = await client.GetStringAsync("/swagger");
        Assert.Contains("swagger-ui", html);
        Assert.Contains("src=\"index.js\"", html);
        var initializer = await client.GetStringAsync("/swagger/index.js");
        Assert.Contains("/openapi/v1.json", initializer);
        using var javascript = await client.GetAsync("/swagger/swagger-ui-bundle.js");
        using var styles = await client.GetAsync("/swagger/swagger-ui.css");
        Assert.Equal(HttpStatusCode.OK, javascript.StatusCode);
        Assert.Equal(HttpStatusCode.OK, styles.StatusCode);
    }

    [Fact]
    public async Task DoesNotExposeOpenApiOrSwaggerOutsideDevelopment()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        await using var production = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        using var client = production.CreateClient();
        using var openapi = await client.GetAsync("/openapi/v1.json");
        using var swagger = await client.GetAsync("/swagger");
        using var courses = await client.GetAsync("/api/courses");
        Assert.Equal(HttpStatusCode.NotFound, openapi.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, swagger.StatusCode);
        Assert.Equal(HttpStatusCode.OK, courses.StatusCode);
    }
}
