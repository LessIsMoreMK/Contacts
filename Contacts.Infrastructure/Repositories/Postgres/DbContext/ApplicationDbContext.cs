using Contacts.Infrastructure.Repositories.Postgres.Configurations;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contacts.Infrastructure.Repositories.Postgres.DbContext;

/// <summary>
/// Class responsible for interacting with the database using Entity Framework.
/// </summary>
public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Properties
    
    public DbSet<UsersDb> Users { get; set; }
    public DbSet<ContactsDb> Contacts { get; set; }
    public DbSet<CategoriesDb> Categories { get; set; }
    public DbSet<SubcategoriesDb> Subcategories { get; set; }

    #endregion
    
    #region Constructors

    public ApplicationDbContext()
    {
    }
		
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    #endregion
    
    #region Methods
    
    /// <summary>
    /// Applies configurations of tables to the model builder.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsersConfiguration());
        modelBuilder.ApplyConfiguration(new ContactsConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
        modelBuilder.ApplyConfiguration(new SubcategoriesConfiguration());
    }

    /// <summary>
    /// Configures the database using provided options.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName))
            .EnableSensitiveDataLogging()
            .UseSnakeCaseNamingConvention();

        base.OnConfiguring(optionsBuilder);
    }
    
    #endregion
}