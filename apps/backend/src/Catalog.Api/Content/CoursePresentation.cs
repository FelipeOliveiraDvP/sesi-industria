namespace Catalog.Api.Content;

public sealed class CoursePresentation
{
    public required string Certification { get; init; }
    public required string DetailsImagePath { get; init; }
    public required CourseTextSection OnlineLessons { get; init; }
    public required CourseTextSectionWithConclusion Assessments { get; init; }
    public required List<CourseFaq> Faqs { get; init; }
    public required LearningPreview LearningPreview { get; init; }
}

public sealed class CourseTextSection
{
    public required string Introduction { get; init; }
    public required List<string> Topics { get; init; }
    public required string Accessibility { get; init; }
}

public sealed class CourseTextSectionWithConclusion
{
    public required string Introduction { get; init; }
    public required List<string> Topics { get; init; }
    public required string Conclusion { get; init; }
}

public sealed class CourseFaq
{
    public required string Question { get; init; }
    public required string Answer { get; init; }
}

public sealed class LearningPreview
{
    public required string Category { get; init; }
    public required int ProgressPercent { get; init; }
    public required string SearchLabel { get; init; }
    public required string BannerImagePath { get; init; }
    public required LearningIntroduction Introduction { get; init; }
    public required List<LearningModule> Modules { get; init; }
    public required LearningArticle Article { get; init; }
    public required LearningArticle Recommendations { get; init; }
}

public sealed class LearningIntroduction
{
    public required string Label { get; init; }
    public required bool Expanded { get; init; }
    public required List<LearningItem> Items { get; init; }
}

public sealed class LearningItem
{
    public required string Label { get; init; }
    public required string State { get; init; }
}

public sealed class LearningModule
{
    public required string Label { get; init; }
    public required bool Expanded { get; init; }
    public required List<LearningLesson> Lessons { get; init; }
}

public sealed class LearningLesson
{
    public required string Label { get; init; }
    public required string Description { get; init; }
}

public sealed class LearningArticle
{
    public required string Title { get; init; }
    public required List<string> Paragraphs { get; init; }
}
