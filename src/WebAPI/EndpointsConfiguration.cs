using WebAPI.Endpoints;

namespace WebAPI;

public static class EndpointsConfiguration
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapContatosEndpoints();

        return app;
    }
}