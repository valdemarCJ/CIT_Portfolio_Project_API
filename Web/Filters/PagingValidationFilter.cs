using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CIT_Portfolio_Project_API.Web.Filters;

public class PagingValidationFilter : IAsyncActionFilter
{
    // Validates common paging query parameters for all endpoints (rejects invalid page/pageSize early).
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var q = context.HttpContext.Request.Query;
        if (q.TryGetValue("page", out var pageStr) && int.TryParse(pageStr, out var page))
        {
            if (page < 1)
            {
                context.Result = new BadRequestObjectResult(new ProblemDetails { Title = "Invalid paging", Detail = "page must be >= 1" });
                return;
            }
        }
        if (q.TryGetValue("pageSize", out var sizeStr) && int.TryParse(sizeStr, out var pageSize))
        {
            if (pageSize < 1 || pageSize > 200)
            {
                context.Result = new BadRequestObjectResult(new ProblemDetails { Title = "Invalid paging", Detail = "pageSize must be between 1 and 200" });
                return;
            }
        }
        // Continue if paging parameters are valid or not provided.
        await next();
    }
}
