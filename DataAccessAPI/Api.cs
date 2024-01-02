namespace DataAccessAPI;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        //All of the API endpoint mapping
        app.MapGet("/Users", GetUsers);
        app.MapGet("/Users/{id}", GetUser);
        app.MapPost("/Users", InsertUser);
        app.MapPut("/Users", UpdateUser);
        app.MapDelete("/Users", DeleteUser);
    }

    private static async Task<IResult> GetUsers(IUserData pData)
    {
        try
        {
            return Results.Ok(await pData.GetUsers());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetUser(int pID, IUserData pData)
    {
        try
        {
            var results = await pData.GetUser(pID);
            if (results == null) return Results.NotFound();
            return Results.Ok(results);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertUser(UserModel pUser, IUserData pData)
    {
        try
        {
            await pData.InsertUser(pUser);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUser(UserModel pUser, IUserData pData)
    {
        try
        {
            await pData.UpdateUser(pUser);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> DeleteUser(int pID, IUserData pData)
    {
        try
        {
            await pData.DeleteUser(pID);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}