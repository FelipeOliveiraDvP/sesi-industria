using Catalog.Api.Data;
using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class SeedInitializationTests(PostgreSqlFixture postgres)
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task MigrationCallbacksSeedIdempotentlyWithoutOverwritingExistingData(bool asynchronous)
    {
        var connection = await postgres.CreateDatabaseAsync();
        await using (var context = PostgreSqlFixture.CreateContext(connection))
        {
            await MigrateAsync(context, asynchronous);
            Assert.Equal(6, await context.Courses.CountAsync());
            Assert.Equal(3, await context.Categories.CountAsync());
            Assert.Equal(2, await context.Modalities.CountAsync());
            Assert.Equal(3, await context.CourseStatuses.CountAsync());

            var course = await context.Courses.SingleAsync(course => course.Id == 1);
            course.Description = "Descrição preservada durante teste de idempotência.";
            context.Categories.Add(new Category { Id = 9000, Name = "Categoria adicional de teste" });
            await context.SaveChangesAsync();
        }

        await using var repeated = PostgreSqlFixture.CreateContext(connection);
        var before = await CountsAsync(repeated);
        await MigrateAsync(repeated, asynchronous);
        Assert.Equal(before, await CountsAsync(repeated));
        Assert.Equal("Descrição preservada durante teste de idempotência.",
            (await repeated.Courses.AsNoTracking().SingleAsync(course => course.Id == 1)).Description);
        Assert.True(await repeated.Categories.AnyAsync(category => category.Id == 9000));
        Assert.Empty(await repeated.Database.GetPendingMigrationsAsync());
    }

    private static Task MigrateAsync(CatalogDbContext context, bool asynchronous)
    {
        if (asynchronous)
        {
            return context.Database.MigrateAsync();
        }
        context.Database.Migrate();
        return Task.CompletedTask;
    }

    private static async Task<int[]> CountsAsync(CatalogDbContext context) =>
    [
        await context.Courses.CountAsync(), await context.Categories.CountAsync(),
        await context.Modalities.CountAsync(), await context.CourseStatuses.CountAsync(),
        await context.CourseModules.CountAsync(), await context.Lessons.CountAsync()
    ];
}
