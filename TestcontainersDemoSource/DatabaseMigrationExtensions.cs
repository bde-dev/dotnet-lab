using System.Data;
using FluentMigrator.Runner;

namespace TestcontainersDemoSource;

public static class DatabaseMigrationExtensions
{
    public static void AddMigrations(this WebApplicationBuilder builder, IDbConnection dbConnection)
    {
        Console.WriteLine("AddMigrations Connection String: " + dbConnection.ConnectionString);
        
        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
                rb.AddPostgres()
                    .WithGlobalConnectionString(dbConnection.ConnectionString)
                    .ScanIn(typeof(PostgresMigration).Assembly).For.Migrations());
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        
        Console.WriteLine("ApplyMigrations Connection String: " +
                          scope.ServiceProvider.GetRequiredService<IDbConnection>().ConnectionString);

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        
        runner.MigrateUp();
    }
}