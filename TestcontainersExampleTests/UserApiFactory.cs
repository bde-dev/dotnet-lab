﻿using System.Data;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Testcontainers.MariaDb;
using Testcontainers.PostgreSql;
using TestcontainersDemoSource;

namespace TestcontainersExampleTests;

//This class demonstrates using test containers to run unit tests for a web API against a postgres container.
//It leverages the WebApplicationFactory as a means to mock the Http client.
//The generic type parameter "Program" tells the WAF what this class is based on.
//IAsyncLifetime allows the class to implement asynchronous startup and clean up tasks.
public class UserApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    //Build a new MariaDB container with a specified config.
    private readonly MariaDbContainer _dbContainer = new MariaDbBuilder()
        .WithImage("mariadb:latest")
        .WithDatabase("userdb")
        .WithUsername("root")
        .WithPassword("admin")
        .Build();

    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("userdb")
        .WithUsername("root")
        .WithPassword("admin")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //This call replaces the services registered in Program.cs with test ones.
        builder.ConfigureTestServices(services =>
        {
            // Remove the services related to the database connection that have been initialized in Program.cs.
            services.RemoveAll(typeof(IDbConnection));
            services.RemoveAll(typeof(IMigrationRunner));
            services.RemoveAll(typeof(AppDbContext));

            // Print the container's connection string.
            var lDbConnection = _postgreSqlContainer.GetConnectionString();
            Console.WriteLine(lDbConnection);
            
            // Add new test services using configurations from the created container.
            services.AddSingleton<IDbConnection>(_ => new NpgsqlConnection(_postgreSqlContainer.GetConnectionString()));
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(lDbConnection));
            
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                    rb.AddPostgres()
                        .WithGlobalConnectionString(_postgreSqlContainer.GetConnectionString())
                        .ScanIn(typeof(PostgresMigration).Assembly).For.Migrations())
                .Configure<SelectingProcessorAccessorOptions>(opt =>
                {
                    opt.ProcessorId = "postgresql";
                });
        });
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }
}