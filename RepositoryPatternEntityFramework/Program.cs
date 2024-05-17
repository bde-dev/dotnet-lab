using Microsoft.EntityFrameworkCore;
using RepositoryPatternEntityFramework;
using RepositoryPatternEntityFramework.Repository;
using RepositoryPatternEntityFramework.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RepositoryPatternEfContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("MariaDB") ??
                         throw new InvalidOperationException("Invalid connection string 'MariaDB'"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();