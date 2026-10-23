using Npgsql;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class PostgreSqlInfrastructureTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task ConnectsToPostgreSql17OnDynamicPort()
    {
        await using var connection = new NpgsqlConnection(postgres.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SHOW server_version_num", connection);

        var version = int.Parse((string)(await command.ExecuteScalarAsync())!);
        Assert.InRange(version, 170000, 179999);
        Assert.NotEqual(5432, connection.Port);
        Assert.Equal("catalog_tests", connection.Database);
    }

    [Fact]
    public async Task CreatesIndependentDatabasesInsideTheTestContainer()
    {
        var first = await postgres.CreateDatabaseAsync();
        var second = await postgres.CreateDatabaseAsync();
        Assert.NotEqual(new NpgsqlConnectionStringBuilder(first).Database,
            new NpgsqlConnectionStringBuilder(second).Database);

        await using var connection = new NpgsqlConnection(first);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT 1", connection);
        Assert.Equal(1, await command.ExecuteScalarAsync());
    }
}
