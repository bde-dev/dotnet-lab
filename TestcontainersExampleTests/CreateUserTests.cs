using System.Net;
using System.Net.Http.Json;
using TestcontainersDemoSource;
using Xunit.Abstractions;

namespace TestcontainersExampleTests;

public class CreateUserTests : IClassFixture<UserApiFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;

    public CreateUserTests(UserApiFactory apiFactory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ServerShouldReturn200_WhenUserIsCreated()
    {
        //Arrange
        var user = new User { Id = 0, FirstName = "Brad", LastName = "Evans" };

        //Act
        var response = await _client.PostAsJsonAsync("users", user);

        //Asset
        var responseString = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}