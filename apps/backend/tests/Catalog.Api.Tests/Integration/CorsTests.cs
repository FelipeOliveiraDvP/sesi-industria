using System.Data.Common;
using System.Net;
using Catalog.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CorsTests(PostgreSqlFixture postgres)
{
    private const string AllowedOrigin = "http://127.0.0.1:5173";

    [Theory]
    [InlineData("/api/courses", HttpStatusCode.OK)]
    [InlineData("/api/courses?categoryId=invalid", HttpStatusCode.BadRequest)]
    [InlineData("/api/courses/999999", HttpStatusCode.NotFound)]
    public async Task AllowsAnyOriginToReadSuccessAndClientErrors(string path, HttpStatusCode expectedStatus)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Add("Origin", AllowedOrigin);

        using var response = await client.SendAsync(request);

        Assert.Equal(expectedStatus, response.StatusCode);
        AssertAllowedOrigin(response);
    }

    [Fact]
    public async Task AllowsAnyOriginOnHandledServerErrors()
    {
        await using var baseFactory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        await using var factory = baseFactory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            services.AddDbContext<CatalogDbContext>(options => options.AddInterceptors(new FailingQueryInterceptor()))));
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/courses");
        request.Headers.Add("Origin", AllowedOrigin);

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        AssertAllowedOrigin(response);
    }

    [Theory]
    [InlineData("http://localhost:5173")]
    [InlineData("https://unlisted.example")]
    public async Task AllowsOtherOriginsAndKeepsCatalogReadOnly(string otherOrigin)
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();

        using var disallowedRequest = new HttpRequestMessage(HttpMethod.Get, "/api/courses");
        disallowedRequest.Headers.Add("Origin", otherOrigin);
        using var disallowedResponse = await client.SendAsync(disallowedRequest);
        Assert.Equal(HttpStatusCode.OK, disallowedResponse.StatusCode);
        Assert.Equal("*", disallowedResponse.Headers.GetValues("Access-Control-Allow-Origin").Single());

        using var writeRequest = new HttpRequestMessage(HttpMethod.Post, "/api/courses");
        writeRequest.Headers.Add("Origin", AllowedOrigin);
        using var writeResponse = await client.SendAsync(writeRequest);
        Assert.Equal(HttpStatusCode.MethodNotAllowed, writeResponse.StatusCode);
    }

    [Fact]
    public async Task AllowsOnlyReadMethodAndNecessaryHeaderOnPreflight()
    {
        await using var factory = new CatalogApiFactory(await postgres.CreateMigratedDatabaseAsync());
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/courses");
        request.Headers.Add("Origin", AllowedOrigin);
        request.Headers.Add("Access-Control-Request-Method", "GET");
        request.Headers.Add("Access-Control-Request-Headers", "Accept");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        AssertAllowedOrigin(response);
        Assert.Contains("GET", response.Headers.GetValues("Access-Control-Allow-Methods").Single());
        Assert.Contains("Accept", response.Headers.GetValues("Access-Control-Allow-Headers").Single(), StringComparison.OrdinalIgnoreCase);
        Assert.False(response.Headers.Contains("Access-Control-Allow-Credentials"));

        using var writePreflight = new HttpRequestMessage(HttpMethod.Options, "/api/courses");
        writePreflight.Headers.Add("Origin", AllowedOrigin);
        writePreflight.Headers.Add("Access-Control-Request-Method", "POST");
        using var rejectedPreflight = await client.SendAsync(writePreflight);
        Assert.DoesNotContain("POST", rejectedPreflight.Headers.GetValues("Access-Control-Allow-Methods").Single());
    }

    private static void AssertAllowedOrigin(HttpResponseMessage response)
    {
        Assert.Equal("*", response.Headers.GetValues("Access-Control-Allow-Origin").Single());
        Assert.False(response.Headers.Contains("Access-Control-Allow-Credentials"));
    }

    private sealed class FailingQueryInterceptor : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("test-only-secret");
    }
}
