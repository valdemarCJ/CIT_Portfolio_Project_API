using System.Linq;
using System.Text.Json;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.DTOs.Auth;
using CIT_Portfolio_Project_API.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CIT_Portfolio_Project_API.Web.Swagger;

public class RequestBodyExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody == null) return;
        if (!operation.RequestBody.Content.TryGetValue("application/json", out var mediaType)) return;

        // Try to detect a [FromBody] parameter type
        var bodyParam = context.MethodInfo
            .GetParameters()
            .FirstOrDefault(p => p.GetCustomAttributes(typeof(FromBodyAttribute), false).Any()
                                 || (!p.ParameterType.IsPrimitive
                                     && p.ParameterType != typeof(string)
                                     && !p.GetCustomAttributes(typeof(FromQueryAttribute), false).Any()
                                     && !p.GetCustomAttributes(typeof(FromRouteAttribute), false).Any()));
        if (bodyParam == null) return;

        object? example = CreateExample(bodyParam.ParameterType);
        if (example == null) return;

        var json = JsonSerializer.Serialize(example, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });
        mediaType.Example = new OpenApiString(json);
    }

    private static object? CreateExample(Type bodyType)
    {
        if (bodyType == typeof(RatingDto))
            return new RatingDto { Tconst = "tt0468569", Value = 8 };
        if (bodyType == typeof(LoginRequest))
            return new LoginRequest { Username = "alice", Password = "P@ssw0rd!" };
        if (bodyType == typeof(UsersController.RegisterRequest))
            return new UsersController.RegisterRequest("testuser", "test@example.com", "P@ssw0rd!");
        if (bodyType == typeof(BookmarksController.AddBookmarkRequest))
            return new BookmarksController.AddBookmarkRequest("tt0468569", "Must watch again soon");
        return null;
    }
}
