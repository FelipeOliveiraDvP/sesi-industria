using Catalog.Api.DTOs;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CoursesController(CourseQueryService courses) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CourseListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseListResponse>> List(
        [FromQuery] CourseQueryParameters parameters, CancellationToken cancellationToken) =>
        Ok(await courses.ListAsync(parameters, cancellationToken));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseDetailDto>> GetDetails(int id, CancellationToken cancellationToken)
    {
        var course = await courses.GetDetailsAsync(id, cancellationToken);
        return course is null
            ? Problem(statusCode: StatusCodes.Status404NotFound, title: "Curso não encontrado.")
            : Ok(course);
    }
}
