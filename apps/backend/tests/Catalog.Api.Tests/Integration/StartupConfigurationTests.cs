using Catalog.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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
    public void RejectsMissingConnectionWithActionableMessage()
    {
        using var factory = new CatalogApiFactory("");
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("ConnectionStrings__DefaultConnection", exception.Message);
        Assert.DoesNotContain("Password=", exception.Message);
    }

    [Fact]
    public async Task DoesNotCreateSchemaOrRunSeedDuringHttpStartup()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var factory = new CatalogApiFactory(connectionString);
        using var client = factory.CreateClient();
        await using var scope = factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        Assert.Single(await context.Database.GetPendingMigrationsAsync());

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public'", connection);
        Assert.Equal(0L, await command.ExecuteScalarAsync());
    }
}
