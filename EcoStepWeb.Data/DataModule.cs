using EcoStepWeb.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcoStepWeb.Data;

public static class DataModule
{
    public static IServiceCollection AddDataModule(this IServiceCollection services, string databaseFolder)
    {
        var connectionString = $"Data Source={databaseFolder}/EcoStepDB.db";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }

    public static async Task<IServiceProvider> SeedMockDataAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");

        await context.Database.MigrateAsync();
        await DataSeeder.SeedMockDataAsync(context);

        return services;
    }
}
