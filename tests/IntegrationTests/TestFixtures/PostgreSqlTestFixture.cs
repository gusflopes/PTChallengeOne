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
        
        // await RunMigrationsAsync();
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
        throw new NotImplementedException();
    }
}