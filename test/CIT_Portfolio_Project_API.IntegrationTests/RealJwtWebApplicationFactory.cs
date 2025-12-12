using DotNetEnv;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CIT_Portfolio_Project_API.IntegrationTests;

/// <summary>
/// WebApplicationFactory that uses the real JwtBearer authentication pipeline
/// (no test auth override). It still loads .env and injects a connection string
/// when POSTGRES_* variables are present.
/// </summary>
public class RealJwtWebApplicationFactory : WebApplicationFactory<Program>
{
    public RealJwtWebApplicationFactory()
    {
        // Best-effort early .env load to populate POSTGRES_* for Program.
        try
        {
            string? dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string? foundRoot = null;
            for (int i = 0; i < 12 && dir != null; i++)
            {
                var apiProj = Path.Combine(dir, "CIT_Portfolio_Project_API.csproj");
                if (File.Exists(apiProj)) { foundRoot = dir; break; }
                dir = Directory.GetParent(dir)?.FullName;
            }

            if (foundRoot != null)
            {
                var dotenv = Path.Combine(foundRoot, ".env");
                if (File.Exists(dotenv))
                {
                    Env.Load(dotenv);
                }
            }
        }
        catch { }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        // No authentication override here: we want real JwtBearer.

    builder.ConfigureAppConfiguration((context, config) =>
        {
            // Ensure .env is loaded for Program connection string selection
            string? dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string? foundRoot = null;
            for (int i = 0; i < 12 && dir != null; i++)
            {
                var apiProj = Path.Combine(dir, "CIT_Portfolio_Project_API.csproj");
                if (File.Exists(apiProj)) { foundRoot = dir; break; }
                dir = Directory.GetParent(dir)?.FullName;
            }
            if (foundRoot != null)
            {
                var dotenv = Path.Combine(foundRoot, ".env");
                if (File.Exists(dotenv))
                {
                    Env.Load(dotenv);
                }
            }

            // If env vars are present, inject explicit ConnectionStrings:Default
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var pw = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(port)
                && !string.IsNullOrWhiteSpace(db) && !string.IsNullOrWhiteSpace(user)
                && !string.IsNullOrWhiteSpace(pw))
            {
                var conn = $"Host={host};Port={port};Database={db};Username={user};Password={pw}";
                var overrides = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Default"] = conn
                };
                config.AddInMemoryCollection(overrides!);
            }
        });
    }
}
