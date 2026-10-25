using System.Net.Http.Json;
using Catalog.Api.DTOs;
using Xunit;

namespace Catalog.Api.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CourseImageTests(PostgreSqlFixture postgres)
{
    [Fact]
    public async Task ResolvesCardAndDetailImagesAgainstEachConfiguredFrontendOrigin()
    {
        var repositoryRoot = FindRepositoryRoot();
        foreach (var assetFile in new[] { "banner-ava.png", "hero-image.png", "video-thumb.png" })
        {
            Assert.True(File.Exists(Path.Combine(repositoryRoot, "apps", "frontend", "public", "assets", assetFile)),
                $"Expected frontend asset {assetFile} to exist in the monorepo.");
        }

        var connection = await postgres.CreateMigratedDatabaseAsync();
        const string firstBase = "http://127.0.0.1:5173";
        const string secondBase = "https://preview.example.test:9443";
        await using var firstFactory = new CatalogApiFactory(connection, $"{firstBase}/");
        await using var secondFactory = new CatalogApiFactory(connection, secondBase);
        using var firstClient = firstFactory.CreateClient();
        using var secondClient = secondFactory.CreateClient();

        var firstList = await firstClient.GetFromJsonAsync<CourseListResponse>("/api/courses");
        var secondList = await secondClient.GetFromJsonAsync<CourseListResponse>("/api/courses");
        Assert.NotNull(firstList);
        Assert.NotNull(secondList);
        Assert.Equal(6, firstList.Items.Count);
        Assert.Equal(firstList.TotalCount, secondList.TotalCount);

        var imagePaths = new Dictionary<int, string>
        {
            [1] = "/assets/banner-ava.png",
            [2] = "/assets/hero-image.png",
            [3] = "/assets/video-thumb.png",
            [4] = "/assets/video-thumb.png",
            [5] = "/assets/video-thumb.png",
            [6] = "/assets/banner-ava.png"
        };

        foreach (var (courseId, imagePath) in imagePaths)
        {
            var firstCourse = Assert.Single(firstList.Items, item => item.Id == courseId);
            var secondCourse = Assert.Single(secondList.Items, item => item.Id == courseId);
            Assert.Equal(firstBase + imagePath, firstCourse.ImageUrl);
            Assert.Equal(secondBase + imagePath, secondCourse.ImageUrl);
            Assert.DoesNotContain("public", firstCourse.ImageUrl, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(firstCourse.FullName, secondCourse.FullName);
            Assert.Equal(firstCourse.Description, secondCourse.Description);
            Assert.Equal(firstCourse.ShortDescription, secondCourse.ShortDescription);

            var firstDetail = await firstClient.GetFromJsonAsync<CourseDetailDto>($"/api/courses/{courseId}");
            var secondDetail = await secondClient.GetFromJsonAsync<CourseDetailDto>($"/api/courses/{courseId}");
            Assert.NotNull(firstDetail);
            Assert.NotNull(secondDetail);
            Assert.Equal(firstBase + imagePath, firstDetail.ImageUrl);
            Assert.Equal(secondBase + imagePath, secondDetail.ImageUrl);
            Assert.Equal(firstBase + "/assets/video-thumb.png", firstDetail.Presentation.DetailsImageUrl);
            Assert.Equal(secondBase + "/assets/video-thumb.png", secondDetail.Presentation.DetailsImageUrl);
            Assert.Equal(firstBase + "/assets/banner-ava.png", firstDetail.Presentation.LearningPreview.BannerImageUrl);
            Assert.Equal(secondBase + "/assets/banner-ava.png", secondDetail.Presentation.LearningPreview.BannerImageUrl);
            Assert.DoesNotContain("public", firstDetail.Presentation.DetailsImageUrl, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("public", firstDetail.Presentation.LearningPreview.BannerImageUrl, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(firstCourse.FullName, firstDetail.FullName);
            Assert.Equal(firstCourse.Description, firstDetail.Description);
            Assert.Equal(firstCourse.ShortDescription, firstDetail.ShortDescription);
        }
    }

    private static string FindRepositoryRoot()
    {
        for (var current = new DirectoryInfo(AppContext.BaseDirectory); current is not null; current = current.Parent)
        {
            if (File.Exists(Path.Combine(current.FullName, "pnpm-workspace.yaml")))
                return current.FullName;
        }

        throw new DirectoryNotFoundException("Could not locate the monorepo root from the test output directory.");
    }
}
