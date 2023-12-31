﻿using Contacts.Infrastructure;
using Contacts.Infrastructure.Helpers;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EndpointBuilder = Contacts.Api.EndpointBuilder;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = configurationBuilder.Build();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(configuration);

builder.Services
    .AddLogging()
    .AddSeqLogger(configuration, builder.Host)
    .AddHttpContextAccessor()
    .RegisterServices()
    .RegisterCommandHandlers()
    .AddMapsterMappings()
    .AddAuthorization();

var app = builder.Build();

var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await dbInitializer.Initialize();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CorsMiddleware>();

EndpointBuilder.BuildApplicationEndpoints(app);

app.Run();
