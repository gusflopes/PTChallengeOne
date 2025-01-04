using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql
                    .EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null)
                    .CommandTimeout(30)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "public")
                    .MigrationsAssembly("Migrations"));
        });

        return services;
    }
}