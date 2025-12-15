using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using CIT_Portfolio_Project_API.Infrastructure.Mapping;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Application.Managers.Implementations;
using CIT_Portfolio_Project_API.Web.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using CIT_Portfolio_Project_API.Web.Swagger;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
	options.Filters.Add<PagingValidationFilter>();
});

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	// Enable JWT Authorize button in Swagger UI
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CIT Portfolio API", Version = "v1" });

	// Define the bearer auth scheme with a proper reference so Swashbuckle applies it to operations
	var jwtScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Indsæt KUN tokenen eller 'Bearer {token}' (UI'en accepterer begge)",
		Reference = new Microsoft.OpenApi.Models.OpenApiReference
		{
			Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
			Id = "Bearer"
		}
	};
	c.AddSecurityDefinition("Bearer", jwtScheme);
	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{ jwtScheme, Array.Empty<string>() }
	});

	// Custom request body examples in Swagger UI
	c.OperationFilter<RequestBodyExamplesOperationFilter>();
});

// Load .env (optional). If .env is missing, Env.Load() is a no-op and we'll fall back to configuration.
Env.Load();

// Prefer a concrete ConnectionStrings:Default if provided via configuration (e.g., tests inject it),
// otherwise try environment variables (POSTGRES_*), and finally fall back to appsettings.*
string connectionString;
string sourceLabel = "appsettings.json";

var configuredConn = builder.Configuration.GetConnectionString("Default");
bool looksPlaceholder = string.IsNullOrWhiteSpace(configuredConn)
	|| configuredConn.Contains("YOUR_DB", StringComparison.OrdinalIgnoreCase)
	|| configuredConn.Contains("YOUR_DEV_DB", StringComparison.OrdinalIgnoreCase)
	|| configuredConn.Contains("YOUR_USER", StringComparison.OrdinalIgnoreCase)
	|| configuredConn.Contains("YOUR_PASSWORD", StringComparison.OrdinalIgnoreCase);

if (!looksPlaceholder)
{
	connectionString = configuredConn!;
	sourceLabel = "configuration";
}
else
{
	// Build connection string from environment variables; if missing, fall back to appsettings.json
	string? host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
	string? port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
	string? db   = Environment.GetEnvironmentVariable("POSTGRES_DB");
	string? user = Environment.GetEnvironmentVariable("POSTGRES_USER");
	string? pw   = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

	if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(port)
		&& !string.IsNullOrWhiteSpace(db) && !string.IsNullOrWhiteSpace(user)
		&& !string.IsNullOrWhiteSpace(pw))
	{
		connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pw}";
		sourceLabel = ".env";
	}
	else
	{
		connectionString = builder.Configuration.GetConnectionString("Default")
							?? throw new InvalidOperationException("Connection string 'Default' not found.");
		sourceLabel = builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json";
	}
}

// EF Core - Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(connectionString)
		   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// AutoMapper
builder.Services.AddAutoMapper(typeof(ApiMappingProfile).Assembly);

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ApiMappingProfile>(); // TODO: replace with actual validators when added

// (No external example providers – using custom operation filter above)

// JWT Auth
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"] ?? "CHANGE_ME_SUPER_SECRET_KEY"));

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidateLifetime = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			IssuerSigningKey = signingKey,
			ClockSkew = TimeSpan.FromMinutes(1)
		};
	});

builder.Services.AddAuthorization();

// Security helpers
builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddSingleton<PasswordHasher>();

// DI: Repositories
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieDetailRepository, MovieDetailRepository>();
builder.Services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
builder.Services.AddScoped<IMoviePersonRepository, MoviePersonRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingReadRepository, RatingReadRepository>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<IWordIndexRepository, WordIndexRepository>();

// DI: Managers (previously Services)
builder.Services.AddScoped<IMovieManager, MovieManager>();
builder.Services.AddScoped<IMovieDetailManager, MovieDetailManager>();
builder.Services.AddScoped<IMovieGenreManager, MovieGenreManager>();
builder.Services.AddScoped<IMoviePersonManager, MoviePersonManager>();
builder.Services.AddScoped<IPersonManager, PersonManager>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IBookmarkManager, BookmarkManager>();
builder.Services.AddScoped<IRatingManager, RatingManager>();
builder.Services.AddScoped<IRatingReadManager, RatingReadManager>();
builder.Services.AddScoped<ISearchManager, SearchManager>();
builder.Services.AddScoped<IAnalyticsManager, AnalyticsManager>();
builder.Services.AddScoped<IWordIndexManager, WordIndexManager>();

// CORS configuration for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder
            .WithOrigins(
                "http://localhost:3000", "https://localhost:3000",  // React default ports
                "http://localhost:3001", "https://localhost:3001",
				"http://localhost:5173", "https://localhost:5173",
				"http://localhost:5174", "https://localhost:5174"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	// Show detailed exceptions in Development
	app.UseDeveloperExceptionPage();

	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "CIT Portfolio API v1");
		// Serve Swagger UI at application root ('/')
		c.RoutePrefix = string.Empty;
	});
}
else
{
	// Generic error handler in non-Development
	app.UseExceptionHandler();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();

// Enable CORS
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Optional: If you prefer Swagger UI under /swagger instead of root,
// comment the RoutePrefix setting above and uncomment this redirect:
// app.MapGet("/", () => Results.Redirect("/swagger"));

// Log which connection source is being used (without secrets)
try
{
	var csb = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
	Console.WriteLine($"DB config source: {sourceLabel} | Host={csb.Host} Port={csb.Port} Database={csb.Database} User={(string.IsNullOrEmpty(csb.Username) ? "(unset)" : csb.Username)}");
}
catch
{
	Console.WriteLine($"DB config source: {sourceLabel}");
}

// Optional DB connectivity probe endpoint (development aid)
app.MapGet("/debug/db-ping", async (AppDbContext dbCtx) =>
{
	try
	{
		var can = await dbCtx.Database.CanConnectAsync();
		return can ? Results.Ok("DB connection OK") : Results.Problem("Cannot connect to DB");
	}
	catch (Exception ex)
	{
		return Results.Problem($"DB connection failed: {ex.Message}");
	}
}).WithTags("Debug");

// Auth debug: return current user's claims (requires valid Authorization header)
app.MapGet("/debug/whoami", (ClaimsPrincipal user) =>
{
	var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
			  ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	var name = user.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
			   ?? user.Identity?.Name;
	var claims = user.Claims.Select(c => new { type = c.Type, value = c.Value });
	return Results.Ok(new { sub, name, claims });
}).RequireAuthorization().WithTags("Debug");





app.Run();

// Expose Program for WebApplicationFactory in integration tests
public partial class Program { }
