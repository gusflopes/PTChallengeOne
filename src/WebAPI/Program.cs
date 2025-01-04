using Core.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/healthcheck", () => new { Message = "OK" })
    .WithName("GetWeatherForecast")
    .WithOpenApi()
    .WithTags("Healthcheck");


var contatosApiGroup = app.MapGroup("/contatos").WithOpenApi().WithTags("Contatos");


contatosApiGroup.MapGet("", () =>
{
    var contatos = new List<Contato>();
    
    return contatos;
    
}).WithName("Listar Contatos");

contatosApiGroup.MapPost("", (CreateContatoRequest request) =>
{
    var contato = new Contato
        { Id = Guid.NewGuid(), Offset = 1, Nome = request.Nome, Email = request.Email, CodigoArea = request.CodigoArea, Telefone = request.Telefone };

    return Results.Created("", contato);
}).WithName("Criar Contato");

contatosApiGroup.MapPut("/{id}", (Guid id, UpdateContatoRequest request) =>
{
    Console.WriteLine($"Atualizar contato com id {id}");
    return Results.NoContent();
}).WithName("Atualizar Contato");

contatosApiGroup.MapGet("/{id}", (Guid id) =>
{
    Console.WriteLine($"Buscar contato com id {id}");
    return Results.NotFound();
}).WithName("Buscar Contato");

contatosApiGroup.MapDelete("/{id}", (Guid id) =>
{
    Console.WriteLine($"Deletar contato com id {id}");
    return Results.NoContent();
}).WithName("Deletar Contato");

app.Run();

public record CreateContatoRequest(string Nome, string Email, string CodigoArea, string Telefone);
public record UpdateContatoRequest(Guid Id, string Nome, string Email, string CodigoArea, string Telefone);