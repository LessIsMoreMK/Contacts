﻿using System.Text;
using Contacts.Application.Commands;
using Contacts.Application.Commands.Categories;
using Contacts.Application.Commands.Categories.Handlers;
using Contacts.Application.Commands.Contacts;
using Contacts.Application.Commands.Contacts.Handlers;
using Contacts.Application.Commands.Users;
using Contacts.Application.Commands.Users.Handlers;
using Contacts.Application.Helpers;
using Contacts.Domain.Repositories;
using Contacts.Domain.Services;
using Contacts.Infrastructure.Repositories;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Contacts.Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            .AddSingleton<LoggerHelpers>()
            .AddTransient<IPasswordService, PasswordService>()
        
            .AddPostgresDb<ApplicationDbContext>(() =>
            {
                services.AddScoped<IUsersRepository, UsersRepository>();
                services.AddScoped<IContactsRepository, ContactsRepository>();
                services.AddScoped<ICategoryRepository, CategoryRepository>();
            })
            
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        services.BuildServiceProvider().GetRequiredService<IConfiguration>()["jwt:key"]!))
                };
            });
        
        return services;
    }
    
    /// <summary>
    /// Registers command handlers.
    /// </summary>
    public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
    {
        services
            .AddTransient<ICommandHandler<AddCategoryRequest>, AddCategoryHandler>()
            .AddTransient<ICommandHandler<AddContactRequest>, AddContactHandler>()
            .AddTransient<ICommandHandler<UpdateContactRequest>, UpdateContactHandler>()
            .AddTransient<ICommandHandler<AddUserRequest>, AddUserHandler>();
        
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
    
    /// <summary>
    /// Adds objects mappings with use of Mapster library.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapsterMappings(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Default
            .NameMatchingStrategy(NameMatchingStrategy.Flexible);
        
        Mappings.Mappings.UsersMappings();
        Mappings.Mappings.ContactsMappings();
        Mappings.Mappings.CategoriesMappings();
        
        return services;
    }
}