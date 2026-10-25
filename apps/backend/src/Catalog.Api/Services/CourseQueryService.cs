using Catalog.Api.Content;
using Catalog.Api.Data;
using Catalog.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Services;

public sealed class CourseQueryService(CatalogDbContext context, CourseImageUrlResolver imageUrls)
{
    public async Task<CourseListResponse> ListAsync(CourseQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = context.Courses.AsNoTracking();
        if (CourseSearch.CreatePattern(parameters.Search) is { } pattern)
        {
            query = query.Where(course =>
                EF.Functions.ILike(course.FullName, pattern, "\\") ||
                EF.Functions.ILike(course.ShortName, pattern, "\\"));
        }
        if (parameters.CategoryId is int categoryId)
        {
            query = query.Where(course => course.CategoryId == categoryId);
        }
        if (parameters.ModalityId is int modalityId)
        {
            query = query.Where(course => course.ModalityId == modalityId);
        }
        if (parameters.StatusId is int statusId)
        {
            query = query.Where(course => course.CourseStatusId == statusId);
        }

        var relativeItems = await query.OrderBy(course => course.FullName).ThenBy(course => course.Id)
            .Select(course => new CourseSummaryDto(
                course.Id, course.FullName, course.ShortName, course.Description, course.ShortDescription,
                new LookupDto(course.Category.Id, course.Category.Name),
                new LookupDto(course.Modality.Id, course.Modality.Name),
                new LookupDto(course.Status.Id, course.Status.Name),
                course.WorkloadHours, course.TargetAudience, course.ImageUrl, course.AvailableOn))
            .ToListAsync(cancellationToken);

        var items = relativeItems.Select(item => item with
        {
            ImageUrl = imageUrls.Resolve(item.ImageUrl)
        }).ToList();

        return new CourseListResponse(items, items.Count);
    }

    public async Task<CourseDetailDto?> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var detail = await context.Courses.AsNoTracking().AsSingleQuery().Where(course => course.Id == id)
            .Select(course => new
            {
                course.Id,
                course.FullName,
                course.ShortName,
                course.Description,
                course.ShortDescription,
                Category = new LookupDto(course.Category.Id, course.Category.Name),
                Modality = new LookupDto(course.Modality.Id, course.Modality.Name),
                Status = new LookupDto(course.Status.Id, course.Status.Name),
                course.WorkloadHours,
                course.TargetAudience,
                course.ImageUrl,
                course.AvailableOn,
                course.PresentationJson,
                Modules = course.Modules.OrderBy(module => module.DisplayOrder).ThenBy(module => module.Id)
                    .Select(module => new CourseModuleDto(
                        module.Id, module.Title, module.Description, module.DisplayOrder,
                        module.Lessons.OrderBy(lesson => lesson.DisplayOrder).ThenBy(lesson => lesson.Id)
                            .Select(lesson => new LessonDto(
                                lesson.Id, lesson.Title, lesson.Description, lesson.DisplayOrder)).ToList()))
                    .ToList()
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (detail is null)
            return null;

        var presentation = CoursePresentationSerializer.Deserialize(detail.PresentationJson);
        return new CourseDetailDto(
            detail.Id, detail.FullName, detail.ShortName, detail.Description, detail.ShortDescription,
            detail.Category, detail.Modality, detail.Status,
            detail.WorkloadHours, detail.TargetAudience, imageUrls.Resolve(detail.ImageUrl), detail.AvailableOn,
            detail.Modules, ToDto(presentation));
    }

    private CoursePresentationDto ToDto(CoursePresentation presentation)
    {
        var learning = presentation.LearningPreview;
        return new CoursePresentationDto(
            presentation.Certification,
            imageUrls.Resolve(presentation.DetailsImagePath),
            new CourseTextSectionDto(presentation.OnlineLessons.Introduction,
                presentation.OnlineLessons.Topics, presentation.OnlineLessons.Accessibility),
            new CourseTextSectionWithConclusionDto(presentation.Assessments.Introduction,
                presentation.Assessments.Topics, presentation.Assessments.Conclusion),
            presentation.Faqs.Select(faq => new CourseFaqDto(faq.Question, faq.Answer)).ToArray(),
            new LearningPreviewDto(
                learning.Category, learning.ProgressPercent, learning.SearchLabel,
                imageUrls.Resolve(learning.BannerImagePath),
                new LearningIntroductionDto(learning.Introduction.Label, learning.Introduction.Expanded,
                    learning.Introduction.Items.Select(item => new LearningItemDto(item.Label, item.State)).ToArray()),
                learning.Modules.Select(module => new LearningModuleDto(
                    module.Label,
                    module.Expanded,
                    module.Lessons.Select(lesson => new LearningLessonDto(lesson.Label, lesson.Description)).ToArray())).ToArray(),
                new LearningArticleDto(learning.Article.Title, learning.Article.Paragraphs),
                new LearningArticleDto(learning.Recommendations.Title, learning.Recommendations.Paragraphs)));
    }
}
