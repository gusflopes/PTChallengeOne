using Core.Entity;

namespace WebAPI.Endpoints;

public record CreateContatoRequest(string Nome, string Email, string CodigoArea, string Telefone);
public record UpdateContatoRequest(Guid Id, string Nome, string Email, string CodigoArea, string Telefone);

public static class ContatosEndpoint
{
    private const string BaseRoute = "contatos";

    public static IEndpointRouteBuilder MapContatosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(BaseRoute)
            .WithTags("Contatos")
            .WithOpenApi();
        
        group.MapGet("", () =>
        {
            var contatos = new List<Contato>();
    
            return contatos;
    
        });

        group.MapPost("", (CreateContatoRequest request) =>
        {
            var contato = new Contato
                { Id = Guid.NewGuid(), Offset = 1, Nome = request.Nome, Email = request.Email, CodigoArea = request.CodigoArea, Telefone = request.Telefone };

            return Results.Created("", contato);
        });

        group.MapPut("/{id}", (Guid id, UpdateContatoRequest request) =>
        {
            Console.WriteLine($"Atualizar contato com id {id}");
            return Results.NoContent();
        });

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