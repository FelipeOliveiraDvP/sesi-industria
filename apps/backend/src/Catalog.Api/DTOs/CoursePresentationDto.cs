namespace Catalog.Api.DTOs;

public sealed record CoursePresentationDto(
    string Certification,
    string DetailsImageUrl,
    CourseTextSectionDto OnlineLessons,
    CourseTextSectionWithConclusionDto Assessments,
    IReadOnlyList<CourseFaqDto> Faqs,
    LearningPreviewDto LearningPreview);

public sealed record CourseTextSectionDto(
    string Introduction,
    IReadOnlyList<string> Topics,
    string Accessibility);

public sealed record CourseTextSectionWithConclusionDto(
    string Introduction,
    IReadOnlyList<string> Topics,
    string Conclusion);

public sealed record CourseFaqDto(string Question, string Answer);

public sealed record LearningPreviewDto(
    string Category,
    int ProgressPercent,
    string SearchLabel,
    string BannerImageUrl,
    LearningIntroductionDto Introduction,
    IReadOnlyList<LearningModuleDto> Modules,
    LearningArticleDto Article,
    LearningArticleDto Recommendations);

public sealed record LearningIntroductionDto(
    string Label,
    bool Expanded,
    IReadOnlyList<LearningItemDto> Items);

public sealed record LearningItemDto(string Label, string State);

public sealed record LearningModuleDto(string Label, bool Expanded, IReadOnlyList<LearningLessonDto> Lessons);

public sealed record LearningLessonDto(string Label, string Description);

public sealed record LearningArticleDto(string Title, IReadOnlyList<string> Paragraphs);
