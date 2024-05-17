using System.Data;
using FluentMigrator.Runner;

namespace RepositoryPatternDapper.Extensions;

public static class DatabaseMigrationExtensions
{
    public static void AddMigrations(this WebApplicationBuilder builder, IDbConnection dbConnection)
    {
        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
                rb.AddMySql5()
                    .WithGlobalConnectionString(dbConnection.ConnectionString)
                    .ScanIn(typeof(DatabaseMigration).Assembly).For.Migrations());
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        var scope = app.Services.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        
        runner.MigrateUp();
    }
}