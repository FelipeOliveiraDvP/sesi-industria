using System.Net;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CourseImageTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task EverySeedImageIsServedLocallyWithoutRemoteResources()
    {
        var connection = await postgres.CreateMigratedDatabaseAsync();
        await using var context = PostgreSqlFixture.CreateContext(connection);
        var courses = await context.Courses.AsNoTracking().ToListAsync();
        await using var factory = new CatalogApiFactory(connection);
        using var client = factory.CreateClient();

        Assert.Equal(6, courses.Count);
        foreach (var course in courses)
        {
            Assert.StartsWith("/images/courses/", course.ImageUrl);
            using var response = await client.GetAsync(course.ImageUrl);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("image/svg+xml", response.Content.Headers.ContentType!.MediaType);
            var svg = XDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Contains(course.ShortName, svg.Root!.Value);
            Assert.DoesNotContain(svg.Descendants().Attributes(), attribute => attribute.Name.LocalName == "href");
            Assert.DoesNotContain(svg.Descendants(), element => element.Name.LocalName == "script");
        }
    }
}
