using Core.Entity;
using WebAPI.Requests;

namespace WebAPI.Endpoints;

public static class ContatosEndpoint
{
    private const string BaseRoute = "/contatos";

    public static IEndpointRouteBuilder MapContatosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{BaseRoute}")
            .WithTags("Contatos")
            .WithOpenApi();
        
        group.MapGet("/", () =>
        {
            var contatos = new List<Contato>();
    
            return contatos;
    
        });

        group.MapPost("/", (CreateContatoRequest request) =>
        {
            var contato = new Contato
                { Id = Guid.NewGuid(), Offset = 1, Nome = request.Nome, Email = request.Email, CodigoArea = request.CodigoArea, Telefone = request.Telefone };

            return Results.Created("/contatos", contato);
        }).WithValidator<CreateContatoRequest>();

        group.MapPut("/{id}", (Guid id, UpdateContatoRequest request) =>
        {
            Console.WriteLine($"Atualizar contato com id {id}");
            return Results.NoContent();
        }).WithValidator<UpdateContatoRequest>();

        group.MapGet("/{id}", (Guid id) =>
        {
            Console.WriteLine($"Buscar contato com id {id}");
            return Results.NotFound();
        });

        group.MapDelete("/{id}", (Guid id) =>
        {
            Console.WriteLine($"Deletar contato com id {id}");
            return Results.NoContent();
        });

        return app;
    }
}