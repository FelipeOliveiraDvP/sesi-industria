using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.Api.Tests.Integration;

public sealed class CatalogApiFactory(
    string connectionString,
    string? frontendUrl = "http://127.0.0.1:5173",
    string? initializeDatabaseOnStartup = null) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("ConnectionStrings:DefaultConnection", connectionString);
        if (frontendUrl is not null)
        {
            builder.UseSetting("FRONTEND_URL", frontendUrl);
        }

        if (initializeDatabaseOnStartup is not null)
        {
            builder.UseSetting("Database:InitializeOnStartup", initializeDatabaseOnStartup);
        }
    }
}
