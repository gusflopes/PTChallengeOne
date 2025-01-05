using Core.Entity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using WebAPI.Configuration;
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
        
        group.MapGet("/", async (AppDbContext dbContext) =>
        {
            var contatos = await dbContext.Contatos.ToListAsync();
    
            return contatos;
    
        });

        group.MapPost("/", async (CreateContatoRequest request, AppDbContext dbContext) =>
        {
            var contato = new Contato
                { Id = Guid.NewGuid(), Nome = request.Nome, Email = request.Email, CodigoArea = request.CodigoArea, Telefone = request.Telefone };
            
            await dbContext.Contatos.AddAsync(contato);
            await dbContext.SaveChangesAsync();

            return Results.Created("/contatos", contato);
        }).WithValidator<CreateContatoRequest>();

        group.MapPut("/{id}", async (Guid id, UpdateContatoRequest request, AppDbContext dbContext) =>
        {
            var contato = await dbContext.Contatos.SingleOrDefaultAsync(x => x.Id == id);
            if (contato == null) return Results.NotFound();
            
            contato.Nome = request.Nome;
            contato.Email = request.Email;
            contato.CodigoArea = request.CodigoArea;
            contato.Telefone = request.Telefone;
            
            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }).WithValidator<UpdateContatoRequest>();

        group.MapGet("/{id}", async (Guid id, AppDbContext dbContext) =>
        {
            var contato = await dbContext.Contatos.SingleOrDefaultAsync(x => x.Id == id);
            return contato == null ? Results.NotFound() : Results.Ok(contato);
        });

        group.MapDelete("/{id}", async (Guid id, AppDbContext dbContext) =>
        {
            var contato = await dbContext.Contatos.SingleOrDefaultAsync(x => x.Id == id);
            if (contato == null)
                return Results.NotFound();
            
            dbContext.Contatos.Remove(contato);
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        });

        return app;
    }
}