using Infrastructure;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace IntegrationTests.TestFixtures;

public class PostgreSqlFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer { get; private set; }
    public string ConnectionString { get; private set; }

    public PostgreSqlFixture()
    {
        PostgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();
        
        await CreateDatabaseAsync();
        
        await RunMigrationsAsync();
        
        var migrationsExecuted = await VerifyMigrationsExecutedAsync();
        if (!migrationsExecuted)
        {
            throw new Exception("Falha ao executar as migrations");
        }
    }

    public async Task DisposeAsync()
    {
        await PostgreSqlContainer.DisposeAsync();
    }

    private async Task CreateDatabaseAsync()
    {
        var masterConnectionString = PostgreSqlContainer.GetConnectionString();
        await using var connection = new NpgsqlConnection(masterConnectionString);
        await connection.OpenAsync();

        var databasename = $"testdb_{Guid.NewGuid().ToString().Replace("-", "")}";
        
        var commandText = $"CREATE DATABASE \"{databasename}\"";
        await using var command = new NpgsqlCommand(commandText, connection);
        await command.ExecuteNonQueryAsync();

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(masterConnectionString)
        {
            Database = databasename
        };
        
        ConnectionString = connectionStringBuilder.ConnectionString;
    }

    private async Task RunMigrationsAsync()
    {
        var services = new ServiceCollection();
        
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"ConnectionStrings:DefaultConnection", ConnectionString}
        });
        
        services.AddInfrastructure(configuration);
        
        await using var serviceProvider = services.BuildServiceProvider();
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
    
    private async Task<bool> VerifyMigrationsExecutedAsync()
    {
        var services = new ServiceCollection();
        
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"ConnectionStrings:DefaultConnection", ConnectionString}
        });
        
        services.AddInfrastructure(configuration);
        
        await using var serviceProvider = services.BuildServiceProvider();
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
        
        return !pendingMigrations.Any() && appliedMigrations.Any();
    }
}