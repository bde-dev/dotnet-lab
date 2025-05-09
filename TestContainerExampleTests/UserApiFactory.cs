using System.Data;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySql.Data.MySqlClient;
using RepositoryPatternDapper;
using Testcontainers.MariaDb;

namespace TestContainerExampleTests;

//This class demonstrates using test containers to run unit tests for a web API against a MariaDB container.
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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //This call replaces the services registered in Program.cs with test ones.
        builder.ConfigureTestServices(services =>
        {
            //Remove the IDbConnection and IMigrationRunner that has been initialized in Program.cs.
            services.RemoveAll(typeof(IDbConnection));
            services.RemoveAll(typeof(IMigrationRunner));
            
            //Add new test services using configurations from the created container.
            services.AddSingleton<IDbConnection>(_ => new MySqlConnection(_dbContainer.GetConnectionString()));
            
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                    rb.AddMySql5()
                        .WithGlobalConnectionString(_dbContainer.GetConnectionString())
                        .ScanIn(typeof(DatabaseMigration).Assembly).For.Migrations())
                .Configure<SelectingProcessorAccessorOptions>(opt =>
                {
                    opt.ProcessorId = "MySql5";
                });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}