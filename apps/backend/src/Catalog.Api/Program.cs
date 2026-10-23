using Catalog.Api.Data;
using Catalog.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings__DefaultConnection no ambiente do processo antes de iniciar a API.");
}

builder.Services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<CourseQueryService>();

var app = builder.Build();
app.UseExceptionHandler(handler => handler.Run(context => Results.Problem(
    statusCode: StatusCodes.Status500InternalServerError,
    title: "Ocorreu um erro inesperado.").ExecuteAsync(context)));
app.UseStatusCodePages(context => Results.Problem(
    statusCode: context.HttpContext.Response.StatusCode).ExecuteAsync(context.HttpContext));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Catalog API v1");
        options.DocumentTitle = "Catálogo de cursos — API";
    });
}

app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program { }
