using Catalog.Api.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[CollectionDefinition(Name)]
public sealed class PostgreSqlCollection : ICollectionFixture<PostgreSqlFixture>
{
    public const string Name = "PostgreSQL";
}

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = new PostgreSqlBuilder("postgres:17")
        .WithDatabase("catalog_tests")
        .WithUsername("catalog_tests")
        .WithPassword("catalog_tests_password")
        .Build();

    public string ConnectionString => container.GetConnectionString();

    public Task InitializeAsync() => container.StartAsync();

    public Task DisposeAsync() => container.DisposeAsync().AsTask();

    public static CatalogDbContext CreateContext(string connectionString) => new(
        new DbContextOptionsBuilder<CatalogDbContext>().UseNpgsql(connectionString).Options);

    public async Task<string> CreateMigratedDatabaseAsync()
    {
        var connectionString = await CreateDatabaseAsync();
        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        return connectionString;
    }

    public async Task<string> CreateDatabaseAsync()
    {
        // Only test-generated identifiers enter this DDL; the container owns every database.
        var database = $"catalog_test_{Guid.NewGuid():N}";
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand($"CREATE DATABASE \"{database}\"", connection);
        await command.ExecuteNonQueryAsync();

        return new NpgsqlConnectionStringBuilder(ConnectionString)
        {
            Database = database,
            Pooling = false
        }.ConnectionString;
    }
}
