using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CIT_Portfolio_Project_API.Web.Filters;

public static class ProblemDetailsConfig
{
    public static IServiceCollection AddDefaultProblemDetails(this IServiceCollection services)
    {
        services.Configure<ProblemDetailsOptions>(options =>
        {
            // TODO: Map custom exceptions to problem details as needed
        });
        return services;
    }
}
