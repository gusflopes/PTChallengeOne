using Npgsql;
using Testcontainers.PostgreSql;

namespace IntegrationTests.TestFixtures;

public class IntegrationTestsFixture : IAsyncLifetime
{
    public PostgreSqlFixture PostgreSqlFixture { get; private set; }
    public CustomWebApplicationFactory<Program> Factory { get; private set; }

    public async Task InitializeAsync()
    {
        PostgreSqlFixture = new PostgreSqlFixture();
        await PostgreSqlFixture.InitializeAsync();
        
        Factory = new CustomWebApplicationFactory<Program>(PostgreSqlFixture);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await PostgreSqlFixture.DisposeAsync();
    }
}