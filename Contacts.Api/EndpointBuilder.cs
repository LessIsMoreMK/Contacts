using Microsoft.AspNetCore.Builder;

namespace Contacts.Api;

public static class EndpointBuilder
{
    public static WebApplication BuildApplicationEndpoints(WebApplication app)
    {
        app.MapGet("/", () => "Contacts API");

        return app;
    }
}