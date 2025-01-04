using Infrastructure;
using WebAPI;

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
    .WithOpenApi()
    .WithTags("Healthcheck");


app.MapEndpoints();

app.Run();
