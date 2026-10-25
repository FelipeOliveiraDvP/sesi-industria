using System.Data.Common;
using Catalog.Api.Data;
using Catalog.Api.Entities;
using Catalog.Api.Options;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Text.Json;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class StartupConfigurationTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task StartsWithConfiguredPostgreSqlConnection()
    {
        await using var factory = new CatalogApiFactory(postgres.ConnectionString);
        using var client = factory.CreateClient();
        await using var scope = factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        Assert.Equal(postgres.ConnectionString, context.Database.GetConnectionString());
        Assert.Equal("Npgsql.EntityFrameworkCore.PostgreSQL", context.Database.ProviderName);
        Assert.True(await context.Database.CanConnectAsync());
    }

    [Fact]
    public void AcceptsFrontendOriginWithOptionalTrailingSlash()
    {
        using var factory = new CatalogApiFactory(postgres.ConnectionString, "http://127.0.0.1:5173/");
        using var client = factory.CreateClient();
        var options = factory.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<FrontendOriginOptions>>();

        Assert.Equal("http://127.0.0.1:5173", FrontendOrigin.Normalize(options.Value.Value));
    }

    [Fact]
    public void RejectsMissingConnectionWithActionableMessage()
    {
        using var factory = new CatalogApiFactory("");
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("ConnectionStrings__DefaultConnection", exception.Message);
        Assert.DoesNotContain("Password=", exception.Message);
    }

    [Fact]
    public void RejectsMissingFrontendOriginAtHttpStartup()
    {
        using var factory = new CatalogApiFactory(postgres.ConnectionString, frontendUrl: null);
        var exception = Assert.ThrowsAny<Exception>(() => factory.CreateClient());

        Assert.Contains("FRONTEND_URL", exception.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("/relative")]
    [InlineData("ftp://127.0.0.1:5173")]
    [InlineData("http://user:password@127.0.0.1:5173")]
    [InlineData("http://127.0.0.1:5173/app")]
    [InlineData("http://127.0.0.1:5173?mode=preview")]
    [InlineData("http://127.0.0.1:5173#preview")]
    public void RejectsInvalidFrontendOriginsAtHttpStartup(string frontendUrl)
    {
        using var factory = new CatalogApiFactory(postgres.ConnectionString, frontendUrl);
        var exception = Assert.ThrowsAny<Exception>(() => factory.CreateClient());

        Assert.Contains("FRONTEND_URL", exception.ToString());
    }

    [Fact]
    public async Task DoesNotCreateSchemaOrRunSeedDuringHttpStartup()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var factory = new CatalogApiFactory(connectionString);
        using var client = factory.CreateClient();
        await using var scope = factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        Assert.Equal(3, (await context.Database.GetPendingMigrationsAsync()).Count());

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public'", connection);
        Assert.Equal(0L, await command.ExecuteScalarAsync());
    }

    [Fact]
    public async Task ExplicitFalseDoesNotCreateSchemaOrRunSeedDuringHttpStartup()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var factory = new CatalogApiFactory(
            connectionString, initializeDatabaseOnStartup: "false");
        using var client = factory.CreateClient();
        await using var scope = factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        Assert.Equal(3, (await context.Database.GetPendingMigrationsAsync()).Count());
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public'", connection);
        Assert.Equal(0L, await command.ExecuteScalarAsync());
    }

    [Fact]
    public async Task RejectsInvalidDatabaseInitializationOptionBeforeCreatingSchema()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var factory = new CatalogApiFactory(
            connectionString, initializeDatabaseOnStartup: "sometimes");

        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());

        Assert.Contains("Database__InitializeOnStartup", exception.Message);
        Assert.DoesNotContain("Password=", exception.Message);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public'", connection);
        Assert.Equal(0L, await command.ExecuteScalarAsync());
    }

    [Fact]
    public async Task RejectsInvalidFrontendOriginBeforeApplyingMigrations()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var factory = new CatalogApiFactory(
            connectionString, frontendUrl: "ftp://invalid.example", initializeDatabaseOnStartup: "true");

        var exception = Assert.ThrowsAny<Exception>(() => factory.CreateClient());

        Assert.Contains("FRONTEND_URL", exception.ToString());
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public'", connection);
        Assert.Equal(0L, await command.ExecuteScalarAsync());
    }

    [Fact]
    public async Task FailsStartupWithoutLeakingConnectionWhenDatabaseIsUnavailable()
    {
        var unavailableConnection = new NpgsqlConnectionStringBuilder(postgres.ConnectionString)
        {
            Port = 1,
            Timeout = 1,
            Pooling = false
        }.ConnectionString;
        await using var factory = new CatalogApiFactory(
            unavailableConnection, initializeDatabaseOnStartup: "true");

        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());

        Assert.Contains("Database initialization failed", exception.Message);
        Assert.DoesNotContain("Password=", exception.ToString(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("catalog_tests_password", exception.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task FailsStartupWithoutLeakingConnectionWhenMigrationPermissionIsMissing()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var role = $"catalog_readonly_{suffix}";
        var databaseName = $"catalog_readonly_{suffix}";
        const string password = "catalog_readonly_test_password";

        await using (var connection = new NpgsqlConnection(postgres.ConnectionString))
        {
            await connection.OpenAsync();
            await using (var createRole = new NpgsqlCommand(
                             $"CREATE ROLE \"{role}\" LOGIN PASSWORD '{password}'", connection))
            {
                await createRole.ExecuteNonQueryAsync();
            }

            await using (var createDatabase = new NpgsqlCommand(
                             $"CREATE DATABASE \"{databaseName}\" OWNER \"catalog_tests\"", connection))
            {
                await createDatabase.ExecuteNonQueryAsync();
            }

            await using var grantConnect = new NpgsqlCommand(
                $"GRANT CONNECT ON DATABASE \"{databaseName}\" TO \"{role}\"", connection);
            await grantConnect.ExecuteNonQueryAsync();
        }

        var adminConnection = new NpgsqlConnectionStringBuilder(postgres.ConnectionString)
        {
            Database = databaseName,
            Pooling = false
        }.ConnectionString;
        await using (var connection = new NpgsqlConnection(adminConnection))
        {
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                $"REVOKE CREATE ON SCHEMA public FROM PUBLIC; GRANT USAGE ON SCHEMA public TO \"{role}\";",
                connection);
            await command.ExecuteNonQueryAsync();
        }

        var restrictedConnection = new NpgsqlConnectionStringBuilder(postgres.ConnectionString)
        {
            Database = databaseName,
            Username = role,
            Password = password,
            Pooling = false
        }.ConnectionString;
        await using var factory = new CatalogApiFactory(
            restrictedConnection, initializeDatabaseOnStartup: "true");

        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());

        Assert.Contains("Database initialization failed", exception.Message);
        Assert.DoesNotContain(password, exception.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("Password=", exception.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FailsStartupWithoutLeakingSeedFailureDetails()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var baseFactory = new CatalogApiFactory(
            connectionString, initializeDatabaseOnStartup: "true");
        await using var failingFactory = baseFactory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services => services.AddDbContext<CatalogDbContext>(
                options => options.AddInterceptors(new FailingSeedInterceptor()))));

        var exception = Assert.Throws<InvalidOperationException>(() => failingFactory.CreateClient());

        Assert.Contains("Database initialization failed", exception.Message);
        Assert.DoesNotContain("seed-test-secret", exception.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("Password=", exception.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AppliesMigrationsAndSeedWithoutFrontendConfiguration()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateDatabaseAsync());

        await context.Database.MigrateAsync();

        Assert.Equal(6, await context.Courses.CountAsync());
        Assert.Equal(3, await context.Categories.CountAsync());
        Assert.Equal(2, await context.Modalities.CountAsync());
        Assert.Equal(3, await context.CourseStatuses.CountAsync());
    }

    [Fact]
    public async Task AppliesMigrationsAndSeedBeforeServingHttpWhenEnabled()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var factory = new CatalogApiFactory(
            connectionString, initializeDatabaseOnStartup: "true");
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/courses");
        response.EnsureSuccessStatusCode();
        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(6, payload.RootElement.GetProperty("items").GetArrayLength());
        Assert.Equal(6, payload.RootElement.GetProperty("totalCount").GetInt32());

        await using var scope = factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        Assert.Equal(6, await context.Courses.CountAsync());
        Assert.Equal(3, await context.Categories.CountAsync());
        Assert.Equal(2, await context.Modalities.CountAsync());
        Assert.Equal(3, await context.CourseStatuses.CountAsync());
        Assert.True(await context.CourseModules.AnyAsync());
        Assert.True(await context.Lessons.AnyAsync());
        Assert.All(await context.Courses.Select(course => course.PresentationJson).ToListAsync(),
            presentation => Assert.False(string.IsNullOrWhiteSpace(presentation)));
        Assert.Empty(await context.Database.GetPendingMigrationsAsync());
    }

    [Fact]
    public async Task RepeatsStartupInitializationWithoutOverwritingExistingData()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        int[] countsAfterFirstStartup;

        await using (var firstFactory = new CatalogApiFactory(
                         connectionString, initializeDatabaseOnStartup: "true"))
        {
            using var client = firstFactory.CreateClient();
            using var response = await client.GetAsync("/api/courses");
            response.EnsureSuccessStatusCode();

            await using var scope = firstFactory.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
            var course = await context.Courses.SingleAsync(item => item.Id == 1);
            course.Description = "Descrição alterada entre inicializações.";
            course.ShortDescription = "Resumo alterado entre inicializações.";
            course.PresentationJson = "{}";
            context.Categories.Add(new Category { Id = 9000, Name = "Categoria preservada" });
            await context.SaveChangesAsync();

            countsAfterFirstStartup =
            [
                await context.Courses.CountAsync(),
                await context.Categories.CountAsync(),
                await context.Modalities.CountAsync(),
                await context.CourseStatuses.CountAsync(),
                await context.CourseModules.CountAsync(),
                await context.Lessons.CountAsync()
            ];
        }

        await using var secondFactory = new CatalogApiFactory(
            connectionString, initializeDatabaseOnStartup: "true");
        using var secondClient = secondFactory.CreateClient();
        using var secondResponse = await secondClient.GetAsync("/api/courses");
        secondResponse.EnsureSuccessStatusCode();

        await using var secondScope = secondFactory.Services.CreateAsyncScope();
        var secondContext = secondScope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        var courseAfterRestart = await secondContext.Courses.AsNoTracking()
            .SingleAsync(item => item.Id == 1);
        Assert.Equal("Descrição alterada entre inicializações.", courseAfterRestart.Description);
        Assert.Equal("Resumo alterado entre inicializações.", courseAfterRestart.ShortDescription);
        Assert.Equal("{}", courseAfterRestart.PresentationJson);
        Assert.True(await secondContext.Categories.AnyAsync(item => item.Id == 9000));
        Assert.Empty(await secondContext.Database.GetPendingMigrationsAsync());

        var countsAfterSecondStartup = new[]
        {
            await secondContext.Courses.CountAsync(),
            await secondContext.Categories.CountAsync(),
            await secondContext.Modalities.CountAsync(),
            await secondContext.CourseStatuses.CountAsync(),
            await secondContext.CourseModules.CountAsync(),
            await secondContext.Lessons.CountAsync()
        };
        Assert.Equal(countsAfterFirstStartup, countsAfterSecondStartup);
    }

    private sealed class FailingSeedInterceptor : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            if (command.CommandText.Contains("\"Categories\"", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("seed-test-secret; Password=must-not-leak");
            }

            return ValueTask.FromResult(result);
        }
    }
}
