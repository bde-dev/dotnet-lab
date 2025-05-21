using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestcontainersDemoSource;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var lDbConnection = new NpgsqlConnection(builder.Configuration.GetConnectionString("postgresql"));

builder.Services.AddScoped<IDbConnection>(_ => lDbConnection);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(lDbConnection));

builder.AddMigrations(lDbConnection);

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEnpdoints();

app.ApplyMigrations();

app.Run();

public partial class Program { }