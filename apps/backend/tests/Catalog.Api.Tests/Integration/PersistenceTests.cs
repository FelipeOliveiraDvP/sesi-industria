using Catalog.Api.Data;
using Catalog.Api.Entities;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class PersistenceTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task AppliesAndReappliesMigrationsWithoutLosingData()
    {
        var connectionString = await postgres.CreateDatabaseAsync();
        await using var context = PostgreSqlFixture.CreateContext(connectionString);
        Assert.Single(await context.Database.GetPendingMigrationsAsync());
        await context.Database.MigrateAsync();
        context.Categories.Add(new Category { Id = 9000, Name = "Categoria de teste" });
        await context.SaveChangesAsync();
        await context.Database.MigrateAsync();
        context.ChangeTracker.Clear();

        Assert.Empty(await context.Database.GetPendingMigrationsAsync());
        Assert.Single(await context.Database.GetAppliedMigrationsAsync());
        Assert.False(context.Database.HasPendingModelChanges());
        Assert.Equal("Categoria de teste", (await context.Categories.FindAsync(9000))!.Name);
        await context.Courses.CountAsync();
        await context.Modalities.CountAsync();
        await context.CourseStatuses.CountAsync();
        await context.CourseModules.CountAsync();
        await context.Lessons.CountAsync();
    }

    [Fact]
    public async Task RoundTripsAllRelationshipsAndAllowsNullAvailability()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        context.ChangeTracker.Clear();

        var course = await context.Courses.Include(course => course.Category)
            .Include(course => course.Modality).Include(course => course.Status)
            .Include(course => course.Modules).ThenInclude(module => module.Lessons)
            .SingleAsync(course => course.Id == 9000);

        Assert.Equal(9000, course.Category.Id);
        Assert.Equal(9000, course.Modality.Id);
        Assert.Equal(9000, course.Status.Id);
        Assert.Null(course.AvailableOn);
        Assert.Equal(12.50m, course.WorkloadHours);
        var module = Assert.Single(course.Modules);
        Assert.Equal(course.Id, module.CourseId);
        Assert.Equal(module.Id, Assert.Single(module.Lessons).CourseModuleId);
    }

    [Theory]
    [InlineData("CategoryId")]
    [InlineData("ModalityId")]
    [InlineData("CourseStatusId")]
    public async Task RejectsInvalidCourseClassification(string column)
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        var exception = await Assert.ThrowsAsync<PostgresException>(() =>
            SetColumnAsync(context, "Courses", column, 999999));
        Assert.Equal(PostgresErrorCodes.ForeignKeyViolation, exception.SqlState);
    }

    [Theory]
    [InlineData("CourseModules", "CourseId")]
    [InlineData("Lessons", "CourseModuleId")]
    public async Task RejectsOrphanedEducationalStructure(string table, string column)
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        var exception = await Assert.ThrowsAsync<PostgresException>(() =>
            SetColumnAsync(context, table, column, 999999));
        Assert.Equal(PostgresErrorCodes.ForeignKeyViolation, exception.SqlState);
    }

    [Theory]
    [InlineData("Categories", "Name")]
    [InlineData("Modalities", "Name")]
    [InlineData("CourseStatuses", "Name")]
    [InlineData("Courses", "FullName")]
    [InlineData("Courses", "ShortName")]
    [InlineData("Courses", "Description")]
    [InlineData("Courses", "CategoryId")]
    [InlineData("Courses", "ModalityId")]
    [InlineData("Courses", "CourseStatusId")]
    [InlineData("Courses", "WorkloadHours")]
    [InlineData("Courses", "TargetAudience")]
    [InlineData("Courses", "ImageUrl")]
    [InlineData("CourseModules", "CourseId")]
    [InlineData("CourseModules", "Title")]
    [InlineData("CourseModules", "Description")]
    [InlineData("CourseModules", "DisplayOrder")]
    [InlineData("Lessons", "CourseModuleId")]
    [InlineData("Lessons", "Title")]
    [InlineData("Lessons", "Description")]
    [InlineData("Lessons", "DisplayOrder")]
    public async Task RejectsNullMandatoryFields(string table, string column)
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        var exception = await Assert.ThrowsAsync<PostgresException>(() =>
            SetColumnAsync(context, table, column, null));
        Assert.Equal(PostgresErrorCodes.NotNullViolation, exception.SqlState);
    }

    [Fact]
    public async Task RejectsDuplicatePrimaryKeys()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        var exception = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlRawAsync(
            "INSERT INTO \"Categories\" (\"Id\", \"Name\") VALUES (9000, 'Duplicada')"));
        Assert.Equal(PostgresErrorCodes.UniqueViolation, exception.SqlState);
    }

    [Fact]
    public async Task EnforcesTextLengthInPostgreSql()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        var exception = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlRawAsync(
            "UPDATE \"Courses\" SET \"FullName\" = {0} WHERE \"Id\" = 9000", new string('a', 201)));
        Assert.Equal(PostgresErrorCodes.StringDataRightTruncation, exception.SqlState);
    }

    [Fact]
    public async Task RestrictsClassificationDeletionAndCascadesOwnedStructure()
    {
        await using var context = PostgreSqlFixture.CreateContext(await postgres.CreateMigratedDatabaseAsync());
        await AddGraphAsync(context);
        var exception = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlRawAsync(
            "DELETE FROM \"Categories\" WHERE \"Id\" = 9000"));
        Assert.Equal(PostgresErrorCodes.ForeignKeyViolation, exception.SqlState);

        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Courses\" WHERE \"Id\" = 9000");
        Assert.False(await context.CourseModules.AnyAsync(module => module.Id == 9000));
        Assert.False(await context.Lessons.AnyAsync(lesson => lesson.Id == 9000));
        Assert.True(await context.Categories.AnyAsync(category => category.Id == 9000));
    }

    private static async Task<int> SetColumnAsync(CatalogDbContext context, string table, string column, int? value)
    {
        using var builder = new NpgsqlCommandBuilder();
        var literal = value?.ToString(CultureInfo.InvariantCulture) ?? "NULL";
        var sql = $"UPDATE {builder.QuoteIdentifier(table)} SET {builder.QuoteIdentifier(column)} = {literal} WHERE \"Id\" = 9000";
        await using var connection = new NpgsqlConnection(context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        return await command.ExecuteNonQueryAsync();
    }

    private static async Task AddGraphAsync(CatalogDbContext context)
    {
        context.AddRange(
            new Category { Id = 9000, Name = "Categoria de teste" },
            new Modality { Id = 9000, Name = "Modalidade de teste" },
            new CourseStatus { Id = 9000, Name = "Status de teste" });
        context.Courses.Add(new Course
        {
            Id = 9000, FullName = "Curso de teste", ShortName = "Teste",
            Description = "Descrição fictícia para teste relacional.",
            CategoryId = 9000, ModalityId = 9000, CourseStatusId = 9000,
            WorkloadHours = 12.50m, TargetAudience = "Público de teste", ImageUrl = "/images/test.svg",
            Modules = [new CourseModule
            {
                Id = 9000, Title = "Módulo de teste", Description = "Descrição do módulo.", DisplayOrder = 1,
                Lessons = [new Lesson
                {
                    Id = 9000, Title = "Aula de teste", Description = "Descrição da aula.", DisplayOrder = 1
                }]
            }]
        });
        await context.SaveChangesAsync();
    }
}
