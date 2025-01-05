using System.Diagnostics.CodeAnalysis;
using Infrastructure;
using WebAPI.Configuration;

namespace WebAPI;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddValidators();

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
            .WithOpenApi()
            .WithTags("Healthcheck");


        app.MapEndpoints();

        app.Run();
    }
}
