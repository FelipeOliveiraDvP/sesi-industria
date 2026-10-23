using Catalog.Api.Data;
using Catalog.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Services;

public sealed class CourseQueryService(CatalogDbContext context)
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

        var items = await query.OrderBy(course => course.FullName).ThenBy(course => course.Id)
            .Select(course => new CourseSummaryDto(
                course.Id, course.FullName, course.ShortName,
                new LookupDto(course.Category.Id, course.Category.Name),
                new LookupDto(course.Modality.Id, course.Modality.Name),
                new LookupDto(course.Status.Id, course.Status.Name),
                course.WorkloadHours, course.TargetAudience, course.ImageUrl, course.AvailableOn))
            .ToListAsync(cancellationToken);

        return new CourseListResponse(items, items.Count);
    }

    public Task<CourseDetailDto?> GetDetailsAsync(int id, CancellationToken cancellationToken = default) =>
        context.Courses.AsNoTracking().AsSingleQuery().Where(course => course.Id == id)
            .Select(course => new CourseDetailDto(
                course.Id, course.FullName, course.ShortName, course.Description,
                new LookupDto(course.Category.Id, course.Category.Name),
                new LookupDto(course.Modality.Id, course.Modality.Name),
                new LookupDto(course.Status.Id, course.Status.Name),
                course.WorkloadHours, course.TargetAudience, course.ImageUrl, course.AvailableOn,
                course.Modules.OrderBy(module => module.DisplayOrder).ThenBy(module => module.Id)
                    .Select(module => new CourseModuleDto(
                        module.Id, module.Title, module.Description, module.DisplayOrder,
                        module.Lessons.OrderBy(lesson => lesson.DisplayOrder).ThenBy(lesson => lesson.Id)
                            .Select(lesson => new LessonDto(
                                lesson.Id, lesson.Title, lesson.Description, lesson.DisplayOrder)).ToList()))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);
}
