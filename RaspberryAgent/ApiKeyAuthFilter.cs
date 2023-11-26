using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaspberryAgent;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
{
    private static readonly string ApiKeyHeaderName = "X-API-KEY";
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var expectedApiKey = Environment.GetEnvironmentVariable("API_KEY");

        if (apiKey != expectedApiKey)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}