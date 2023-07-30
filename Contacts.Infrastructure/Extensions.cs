using Contacts.Domain.Repositories;
using Contacts.Infrastructure.Repositories.Postgres;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Contacts.Infrastructure;

/// <summary>
/// Extension class for service registration and application setup.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Registering all the necessary services for the application.
    /// </summary>
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .AddSingleton<DatabaseInitializer>()
        
            .AddPostgresDb<ApplicationDbContext>(() =>
            {
                services.AddScoped<IContactsRepository, ContactsRepository>();
            });
            
        return services;
    }
    
    /// <summary>
    /// Adding the PostgreSQL database context and executing the repository configuration action.
    /// </summary>
    private static IServiceCollection AddPostgresDb<T>(this IServiceCollection services, Action configureRepositories)
        where T : DbContext
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetValue<string>("postgres:connectionString");
        services.AddDbContext<T>(options => options.UseNpgsql(connectionString));
        configureRepositories();
        return services;
    }
    
    /// <summary>
    /// Adds Seq logger to the application when enabled in the configuration.
    /// </summary>
    public static IServiceCollection AddSeqLogger(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder)
    {
        if (!configuration.GetValue<bool>("seq:enabled"))
            return services;
        
        var serilogConfiguration = new LoggerConfiguration().Enrich.FromLogContext();
        serilogConfiguration.WriteTo.Seq(configuration.GetValue<string>("seq:url")!);
        Log.Logger = serilogConfiguration.CreateLogger();
        
        hostBuilder.UseSerilog();

        return services;
    }
}