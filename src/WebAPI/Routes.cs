using WebAPI.Endpoints;

namespace WebAPI;

public static class Routes
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapContatosEndpoints();

        return app;
    }
}