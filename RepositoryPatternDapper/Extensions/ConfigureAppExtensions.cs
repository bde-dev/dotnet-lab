namespace RepositoryPatternDapper;

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

    private static async Task<IResult> CreateUser(IUserService userService, User user)
    {
        try
        {
            await userService.CreateUser(user);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }

    private static async Task<IResult> UpdateUser(IUserService userService, User user)
    {
        try
        {
            await userService.UpdateUser(user);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }

    private static async Task<IResult> DeleteUser(IUserService userService, int id)
    {
        try
        {
            await userService.DeleteUser(id);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }
}