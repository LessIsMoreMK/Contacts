using Microsoft.AspNetCore.Http;

namespace Contacts.Infrastructure.Helpers;

public class CorsMiddleware
{
    private readonly RequestDelegate _next;

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

        if (context.Request.Method == "OPTIONS")
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("OK");
        }
        else
        {
            await _next(context);
        }
    }
}