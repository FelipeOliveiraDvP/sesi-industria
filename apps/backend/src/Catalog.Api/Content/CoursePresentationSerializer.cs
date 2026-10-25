using System.Text.Json;
using System.Text.Json.Serialization;

namespace Catalog.Api.Content;

public static class CoursePresentationSerializer
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    public static string Serialize(CoursePresentation presentation)
    {
        ArgumentNullException.ThrowIfNull(presentation);
        Validate(presentation);
        return JsonSerializer.Serialize(presentation, Options);
    }

    public static CoursePresentation Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new JsonException("Course presentation JSON is required.");

        var presentation = JsonSerializer.Deserialize<CoursePresentation>(json, Options)
            ?? throw new JsonException("Course presentation JSON cannot be null.");
        Validate(presentation);
        return presentation;
    }

    private static void Validate(CoursePresentation presentation)
    {
        RequireText(presentation.Certification, "certification");
        RequireAssetPath(presentation.DetailsImagePath, "detailsImagePath");

        var lessons = presentation.OnlineLessons
            ?? throw Missing("onlineLessons");
        RequireText(lessons.Introduction, "onlineLessons.introduction");
        RequireTexts(lessons.Topics, "onlineLessons.topics");
        RequireText(lessons.Accessibility, "onlineLessons.accessibility");

        var assessments = presentation.Assessments
            ?? throw Missing("assessments");
        RequireText(assessments.Introduction, "assessments.introduction");
        RequireTexts(assessments.Topics, "assessments.topics");
        RequireText(assessments.Conclusion, "assessments.conclusion");

        if (presentation.Faqs is null || presentation.Faqs.Count != 10)
            throw Missing("faqs (exactly ten entries)");
        for (var index = 0; index < presentation.Faqs.Count; index++)
        {
            var faq = presentation.Faqs[index] ?? throw Missing($"faqs[{index}]");
            RequireText(faq.Question, $"faqs[{index}].question");
            RequireText(faq.Answer, $"faqs[{index}].answer");
        }

        var learning = presentation.LearningPreview
            ?? throw Missing("learningPreview");
        RequireText(learning.Category, "learningPreview.category");
        if (learning.ProgressPercent != 10)
            throw new JsonException("learningPreview.progressPercent must be the fixed demonstration value 10.");
        RequireText(learning.SearchLabel, "learningPreview.searchLabel");
        RequireAssetPath(learning.BannerImagePath, "learningPreview.bannerImagePath");

        var introduction = learning.Introduction
            ?? throw Missing("learningPreview.introduction");
        RequireText(introduction.Label, "learningPreview.introduction.label");
        if (introduction.Items is null || introduction.Items.Count != 3)
            throw Missing("learningPreview.introduction.items (exactly three entries)");
        for (var index = 0; index < introduction.Items.Count; index++)
        {
            var item = introduction.Items[index] ?? throw Missing($"learningPreview.introduction.items[{index}]");
            RequireText(item.Label, $"learningPreview.introduction.items[{index}].label");
            RequireText(item.State, $"learningPreview.introduction.items[{index}].state");
            if (item.State is not ("completed" or "current" or "pending"))
                throw new JsonException($"learningPreview.introduction.items[{index}].state is not supported.");
        }

        if (learning.Modules is null || learning.Modules.Count != 6)
            throw Missing("learningPreview.modules (exactly six entries)");
        for (var index = 0; index < learning.Modules.Count; index++)
        {
            var module = learning.Modules[index] ?? throw Missing($"learningPreview.modules[{index}]");
            RequireText(module.Label, $"learningPreview.modules[{index}].label");
            if (module.Lessons is null || module.Lessons.Count != 2)
                throw Missing($"learningPreview.modules[{index}].lessons (exactly two entries)");
            for (var lessonIndex = 0; lessonIndex < module.Lessons.Count; lessonIndex++)
            {
                var lesson = module.Lessons[lessonIndex]
                    ?? throw Missing($"learningPreview.modules[{index}].lessons[{lessonIndex}]");
                RequireText(lesson.Label, $"learningPreview.modules[{index}].lessons[{lessonIndex}].label");
                RequireText(lesson.Description, $"learningPreview.modules[{index}].lessons[{lessonIndex}].description");
            }
        }

        ValidateArticle(learning.Article, "learningPreview.article", expectedParagraphs: 3);
        ValidateArticle(learning.Recommendations, "learningPreview.recommendations", expectedParagraphs: 4);
    }

    private static void ValidateArticle(LearningArticle? article, string path, int expectedParagraphs)
    {
        if (article is null)
            throw Missing(path);
        RequireText(article.Title, $"{path}.title");
        if (article.Paragraphs is null || article.Paragraphs.Count != expectedParagraphs)
            throw Missing($"{path}.paragraphs (exactly {expectedParagraphs} entries)");
        RequireTexts(article.Paragraphs, $"{path}.paragraphs");
    }

    private static void RequireTexts(List<string>? values, string path)
    {
        if (values is null || values.Count == 0)
            throw Missing(path);

        for (var index = 0; index < values.Count; index++)
            RequireText(values[index], $"{path}[{index}]");
    }

    private static void RequireText(string? value, string path)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw Missing(path);
        if (value.Contains('<') || value.Contains('>'))
            throw new JsonException($"{path} must be plain text and cannot contain HTML markup.");
    }

    private static void RequireAssetPath(string? value, string path)
    {
        RequireText(value, path);
        if (!value!.StartsWith("/assets/", StringComparison.Ordinal) ||
            value.Contains("..", StringComparison.Ordinal) ||
            value.Contains('?', StringComparison.Ordinal) ||
            value.Contains('#', StringComparison.Ordinal) ||
            value.Contains(':', StringComparison.Ordinal))
            throw new JsonException($"{path} must be a local public path under /assets/.");
    }

    private static JsonException Missing(string path) =>
        new($"Required course presentation content is missing or invalid: {path}.");
}
