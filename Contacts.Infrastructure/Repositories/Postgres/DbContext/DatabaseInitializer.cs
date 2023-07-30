using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure.Repositories.Postgres.DbContext;

/// <summary>
/// Provides functionality to initialize the database.
/// </summary>
public class DatabaseInitializer
{
    #region Fileds
    
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    
    #endregion
    
    #region Constructors
    
    public DatabaseInitializer(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Initializes the database by performing any pending migrations when enabled in the configuration.
    /// </summary>
    public async Task Initialize()
    {
        if (_configuration.GetValue<bool>("postgres:migrateOnStartup"))
            await CreateDbIfNotExists(_serviceProvider);
    }
    
    #endregion
    
    #region Priavate Helpers
    
    /// <summary>
    /// Creates the database if it does not exist, and performs any pending migrations.
    /// </summary>
    /// <param name="serviceProvider">A service provider.</param>
    private static async Task CreateDbIfNotExists(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
    
    #endregion
}