using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DotNetEnv;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace CIT_Portfolio_Project_API.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestWebApplicationFactory()
    {
        // Eagerly load .env and set POSTGRES_* env vars BEFORE the host is built,
        // so Program.cs sees them when constructing the connection string.
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
                    // Load into process env
                    Env.Load(dotenv);

                    // Additionally ensure variables are set explicitly
                    var kvs = File.ReadAllLines(dotenv)
                        .Where(l => !string.IsNullOrWhiteSpace(l) && !l.TrimStart().StartsWith("#"))
                        .Select(l => l.Split('=', 2))
                        .Where(parts => parts.Length == 2)
                        .Select(parts => new { Key = parts[0].Trim(), Value = parts[1].Trim() });

                    foreach (var kv in kvs)
                    {
                        if (kv.Key.StartsWith("POSTGRES_", StringComparison.OrdinalIgnoreCase))
                        {
                            Environment.SetEnvironmentVariable(kv.Key, kv.Value);
                        }
                    }
                }
            }
        }
        catch
        {
            // best effort; we'll also try during ConfigureAppConfiguration
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        builder.ConfigureServices(services =>
        {
            // Override authentication to use test scheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, options => { });
        });

        // Load .env from repository root or API project root so Program can build the real connection string
        builder.ConfigureAppConfiguration((context, config) =>
        {
            string? dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string? foundRoot = null;
            for (int i = 0; i < 12 && dir != null; i++)
            {
                // Detect API project root by the presence of the main csproj
                var apiProj = Path.Combine(dir, "CIT_Portfolio_Project_API.csproj");
                if (File.Exists(apiProj))
                {
                    foundRoot = dir;
                    break;
                }
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

            // If env vars are present, inject an explicit ConnectionStrings:Default to ensure Program uses the real DB
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
