using CachedDecoratorPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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


//Couple of ways to register the cache.
//1. Very simply:
//builder.Services.AddScoped<UserRepository>();
//builder.Services.AddScoped<IUserRepository, CachedUserRepository>();


//2. Bit more involved:
//builder.Services.AddScoped<UserRepository>();
// builder.Services.AddScoped<IUserRepository>(provider =>
// {
//     var repository = provider.GetRequiredService<UserRepository>();
//
//     var cache = provider.GetRequiredService<IMemoryCache>();
//
//     return new CachedUserRepository(repository, cache);
// });


//3. Decorate explicitly with Scrutor:
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Decorate<IUserRepository, CachedUserRepository>();



builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCachedEndpoints();

app.Run();