namespace TestcontainersDemoSource;

public static class MapEndpoints
{
    public static void MapEnpdoints(this WebApplication app)
    {
        app.MapGet("/users", GetAllUsers);
        app.MapPost("/users", CreateUser);
    }

    private static async Task<IResult> GetAllUsers(IUserService userService)
    {
        try
        {
            return Results.Ok(await userService.GetUsers());
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
            userService.AddUser(user);
            return Results.Ok();
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }
}