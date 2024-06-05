using System.Data;
using MySql.Data.MySqlClient;
using Polly;
using Polly.Retry;
using RepositoryPatternDapper;
using RepositoryPatternDapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnection = new MySqlConnection(builder.Configuration.GetConnectionString("MariaDB"));

Console.WriteLine("Got Connection String: " + dbConnection.ConnectionString);

builder.Services.AddScoped<IDbConnection>(_ => dbConnection);

builder.Services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));

builder.Services.AddScoped<IUserService, UserService>();

builder.AddMigrations(dbConnection);

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

await app.WaitForDb();

app.ApplyMigrations();

app.Run();

public partial class Program { }