using System.Data;
using IntegrationTests.TestFixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly PostgreSqlFixture _databaseFixture;

    public CustomWebApplicationFactory(PostgreSqlFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var connectionString = _databaseFixture.ConnectionString;
            
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", connectionString },
                // { "Logging:LogLevel:Default", "Debug" },
                // { "Logging:LogLevel:Microsoft", "Debug" },
                // { "Logging:LogLevel:Microsoft.Hosting.Lifetime", "Information" },
                // { "Logging:LogLevel:System", "Information" },
                // { "Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information" }
            });
            
        });
        
        builder.ConfigureServices((context, services) =>
        {
            var configManager = new ConfigurationManager();
            configManager.AddConfiguration(context.Configuration);

            services.AddScoped<IDbConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new NpgsqlConnection(connectionString);
            });
            
            // registrar os demais serviços (layers)
        });
        
        builder.UseEnvironment("Testing");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var configuration = scopedServices.GetRequiredService<IConfiguration>();

                // Configurações adicionais?
            }
        });
        
        return base.CreateHost(builder);
    }
    
}
