using Catalog.Api.Data;
using Catalog.Api.Options;
using Catalog.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
const string initializeDatabaseKey = "Database:InitializeOnStartup";
var initializeDatabaseValue = builder.Configuration[initializeDatabaseKey];
if (initializeDatabaseValue is not null && !bool.TryParse(initializeDatabaseValue, out _))
{
    throw new InvalidOperationException(
        "Database__InitializeOnStartup must be either 'true' or 'false'.");
}
var initializeDatabaseOnStartup = bool.TryParse(initializeDatabaseValue, out var initializeDatabase) &&
                                  initializeDatabase;

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddOptions<FrontendOriginOptions>()
    .Configure(options => options.Value = builder.Configuration[FrontendOriginOptions.ConfigurationKey] ?? string.Empty)
    .Validate(options => FrontendOrigin.TryParse(options.Value, out _),
        "FRONTEND_URL is required and must be an absolute HTTP or HTTPS origin without credentials, path, query, or fragment.")
    .ValidateOnStart();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings__DefaultConnection no ambiente do processo antes de iniciar a API.");
}

builder.Services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddSingleton<CourseImageUrlResolver>();
builder.Services.AddScoped<CourseQueryService>();

var app = builder.Build();
app.UseRouting();
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .WithMethods("GET")
    .WithHeaders("Accept"));
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

if (initializeDatabaseOnStartup)
{
    _ = app.Services
        .GetRequiredService<Microsoft.Extensions.Options.IOptions<FrontendOriginOptions>>()
        .Value;

    await using var scope = app.Services.CreateAsyncScope();
    var database = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    try
    {
        await database.Database.MigrateAsync();
    }
    catch (Exception exception)
    {
        app.Logger.LogCritical(
            "Database initialization failed while applying migrations or seed. Failure type: {FailureType}",
            exception.GetType().FullName);
        throw new InvalidOperationException(
            "Database initialization failed while applying migrations and seed; the API cannot start.");
    }
}

await app.RunAsync();

public partial class Program { }
