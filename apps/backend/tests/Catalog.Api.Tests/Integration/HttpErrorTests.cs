using System.Data.Common;
using System.Net;
using System.Net.Http.Json;
using Catalog.Api.Data;
using Catalog.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class HttpErrorTests(PostgreSqlFixture postgres)
{
    [Theory]
    [InlineData("/api/courses?categoryId=abc", 400)]
    [InlineData("/api/courses/abc", 400)]
    [InlineData("/api/courses/999999", 404)]
    [InlineData("/rota-inexistente", 404)]
    public async Task ClientErrorsHaveConsistentProblemDetails(string path, int status)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var response = await client.GetAsync(path);
        Assert.Equal(status, (int)response.StatusCode);
        await AssertProblemAsync(response, status);
    }

    [Theory]
    [InlineData("POST", "/api/courses")]
    [InlineData("POST", "/api/courses/1")]
    [InlineData("PUT", "/api/courses/1")]
    [InlineData("PATCH", "/api/courses/1")]
    [InlineData("DELETE", "/api/courses/1")]
    public async Task WriteMethodsAreUnavailableAndDoNotMutateTheCatalog(string method, string path)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        var before = await client.GetFromJsonAsync<CourseDetailDto>("/api/courses/1");
        using var request = new HttpRequestMessage(new HttpMethod(method), path)
        {
            Content = JsonContent.Create(new { fullName = "Alteração não permitida" })
        };
        using var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        await AssertProblemAsync(response, 405);
        var after = await client.GetFromJsonAsync<CourseDetailDto>("/api/courses/1");
        Assert.Equal(before!.FullName, after!.FullName);
        Assert.Equal(before.Description, after.Description);
        Assert.Equal(6, (await client.GetFromJsonAsync<CourseListResponse>("/api/courses"))!.TotalCount);
    }

    [Theory]
    [InlineData("application/json")]
    [InlineData("text/html")]
    [InlineData("application/xml")]
    public async Task UnexpectedExceptionIsGenericEvenInDevelopmentAndForNonJsonAccept(string accept)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        await using var failing = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            services.AddDbContext<CatalogDbContext>(options => options.AddInterceptors(new FailingQueryInterceptor()))));
        using var client = failing.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/courses");
        request.Headers.Accept.ParseAdd(accept);
        using var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        await AssertProblemAsync(response, 500);
        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("test-only-secret", body);
        Assert.DoesNotContain("InvalidOperationException", body);
        Assert.DoesNotContain("FailingQueryInterceptor", body);
        Assert.DoesNotContain("stackTrace", body, StringComparison.OrdinalIgnoreCase);
    }

    private static async Task AssertProblemAsync(HttpResponseMessage response, int status)
    {
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(status, problem.Status);
        Assert.False(string.IsNullOrWhiteSpace(problem.Title));
        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("Password=", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ConnectionStrings", body, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class FailingQueryInterceptor : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("test-only-secret; Password=should-not-leak");
    }
}
