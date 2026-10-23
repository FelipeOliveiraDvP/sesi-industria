using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.DTOs;

public sealed class CourseQueryParameters
{
    [FromQuery(Name = "search")]
    public string? Search { get; set; }
    [FromQuery(Name = "categoryId")]
    public int? CategoryId { get; set; }
    [FromQuery(Name = "modalityId")]
    public int? ModalityId { get; set; }
    [FromQuery(Name = "statusId")]
    public int? StatusId { get; set; }
}
