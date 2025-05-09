using System.Net;
using System.Net.Http.Json;
using RepositoryPatternDapper;
using Xunit.Abstractions;

namespace TestContainerExampleTests;

public class GetAllUsersTests : IClassFixture<UserApiFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;

    public GetAllUsersTests(ITestOutputHelper testOutputHelper, UserApiFactory _userApiFactory)
    {
        _client = _userApiFactory.CreateClient();
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task GetAll_ReturnsEmptyResult_WhenNoUsersExist()
    {
        //Act
        var response = await _client.GetAsync("users");
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        foreach (var user in users)
        {
            _testOutputHelper.WriteLine(user.ToString());
        }
        Assert.Empty(users);
    }
}