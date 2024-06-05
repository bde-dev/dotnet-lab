using System.Data;
using MySql.Data.MySqlClient;
using Polly;

namespace RepositoryPatternDapper;

public static class PollyExtensions
{
    public static async Task WaitForDb(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        
        var retryStrategy = new ResiliencePipelineBuilder().AddRetry(new()
            {
                ShouldHandle = new PredicateBuilder().Handle<MySqlException>(),
                MaxRetryAttempts = int.MaxValue,
                Delay = TimeSpan.FromSeconds(5),
                OnRetry = args =>
                {
                    Console.WriteLine("Couldn't connect to database: " + dbConnection.ConnectionString);
                    Console.WriteLine("Error: " + args.Outcome.Exception!.Message);
                    Console.WriteLine("Retrying in 5 seconds...");
                    return default;
                }
            })
            .Build();

        await retryStrategy.ExecuteAsync(async token =>
        {
            await Task.Delay(100, token);
            Console.WriteLine("Attempting to connect to database: " + dbConnection.ConnectionString);
            dbConnection.Open();
            Console.WriteLine("Successfully connected to databases. Continuing to apply migrations.");
        });
    }
}