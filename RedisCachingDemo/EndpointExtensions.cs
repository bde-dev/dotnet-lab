using RepositoryPatternEntityFramework.Services;

namespace RedisCachingDemo;

public static class EndpointExtensions
{
    public static void MapCachedEndpoints(this WebApplication app)
    {
        app.MapGet("users/{id}", GetUserById);
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
}