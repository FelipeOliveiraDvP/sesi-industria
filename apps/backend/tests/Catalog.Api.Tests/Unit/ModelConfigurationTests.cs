using Catalog.Api.Configurations;
using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace Catalog.Api.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class ModelConfigurationTests
{
    private static readonly IMutableModel Model = new ModelBuilder()
        .ApplyConfigurationsFromAssembly(typeof(CourseConfiguration).Assembly).Model;

    [Fact]
    public void UsesSixEntitiesWithExplicitIntegerKeys()
    {
        Assert.Equal(6, Model.GetEntityTypes().Count());
        foreach (var entity in Model.GetEntityTypes())
        {
            var key = Assert.Single(entity.FindPrimaryKey()!.Properties);
            Assert.Equal(typeof(int), key.ClrType);
            Assert.Equal(ValueGenerated.Never, key.ValueGenerated);
        }
    }

    [Theory]
    [InlineData(typeof(Course), nameof(Course.FullName), 200)]
    [InlineData(typeof(Course), nameof(Course.ShortName), 80)]
    [InlineData(typeof(Course), nameof(Course.Description), 4000)]
    [InlineData(typeof(Course), nameof(Course.ShortDescription), 1000)]
    [InlineData(typeof(Course), nameof(Course.TargetAudience), 1000)]
    [InlineData(typeof(Course), nameof(Course.ImageUrl), 512)]
    [InlineData(typeof(Category), nameof(Category.Name), 120)]
    [InlineData(typeof(Modality), nameof(Modality.Name), 120)]
    [InlineData(typeof(CourseStatus), nameof(CourseStatus.Name), 120)]
    [InlineData(typeof(CourseModule), nameof(CourseModule.Title), 200)]
    [InlineData(typeof(CourseModule), nameof(CourseModule.Description), 4000)]
    [InlineData(typeof(Lesson), nameof(Lesson.Title), 200)]
    [InlineData(typeof(Lesson), nameof(Lesson.Description), 4000)]
    public void RequiresTextAndLimitsItsLength(Type entity, string name, int length)
    {
        var property = Model.FindEntityType(entity)!.FindProperty(name)!;
        Assert.False(property.IsNullable);
        Assert.Equal(length, property.GetMaxLength());
    }

    [Fact]
    public void RequiresPresentationJsonAsJsonb()
    {
        var property = Model.FindEntityType(typeof(Course))!
            .FindProperty(nameof(Course.PresentationJson))!;

        Assert.False(property.IsNullable);
        Assert.Equal("jsonb", property.GetColumnType());
    }

    [Fact]
    public void ConfiguresRequiredRelationshipsAndOrderedIndexes()
    {
        var course = Model.FindEntityType(typeof(Course))!;
        Assert.Equal(3, course.GetForeignKeys().Count());
        Assert.All(course.GetForeignKeys(), key =>
        {
            Assert.True(key.IsRequired);
            Assert.Equal(DeleteBehavior.Restrict, key.DeleteBehavior);
        });

        foreach (var type in new[] { typeof(CourseModule), typeof(Lesson) })
        {
            var entity = Model.FindEntityType(type)!;
            var foreignKey = Assert.Single(entity.GetForeignKeys());
            Assert.True(foreignKey.IsRequired);
            Assert.Equal(DeleteBehavior.Cascade, foreignKey.DeleteBehavior);
            Assert.Contains(entity.GetIndexes(), index => index.Properties.Count == 2 &&
                index.Properties[1].Name == "DisplayOrder");
        }
    }

    [Fact]
    public void AllowsOptionalDateAndDoesNotInventBusinessConstraints()
    {
        var course = Model.FindEntityType(typeof(Course))!;
        Assert.True(course.FindProperty(nameof(Course.AvailableOn))!.IsNullable);
        var hours = course.FindProperty(nameof(Course.WorkloadHours))!;
        Assert.Equal(7, hours.GetPrecision());
        Assert.Equal(2, hours.GetScale());
        foreach (var entity in Model.GetEntityTypes())
        {
            Assert.DoesNotContain(entity.GetIndexes(), index => index.IsUnique);
            Assert.Empty(entity.GetCheckConstraints());
        }
    }
}
