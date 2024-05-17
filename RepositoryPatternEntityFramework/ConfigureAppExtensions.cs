using RepositoryPatternEntityFramework.Models;
using RepositoryPatternEntityFramework.Services;

namespace RepositoryPatternEntityFramework;

public static class ConfigureAppExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/users", GetAllUsers);
        app.MapGet("/users/{id}", GetUserById);
        app.MapPost("/users", CreateUser);
        app.MapPut("/users", UpdateUser);
        app.MapDelete("/users", DeleteUser);
    }

    private static async Task<IResult> GetAllUsers(IUserService userService)
    {
        try
        {
            return Results.Ok(await userService.GetAllUsers());
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }
    
    private static async Task<IResult> GetUserById(IUserService userService, int id)
    {
        try
        {
            return Results.Ok(await userService.GetUserById(id));
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }

    private static IResult CreateUser(IUserService userService, User user)
    {
        try
        {
            userService.CreateUser(user);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }

    private static IResult UpdateUser(IUserService userService, User user)
    {
        try
        {
            userService.UpdateUser(user);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }

    private static IResult DeleteUser(IUserService userService, int id)
    {
        try
        {
            var user = userService.GetUserById(id).Result;

            if (user == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }
            
            userService.DeleteUser(user);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }
}