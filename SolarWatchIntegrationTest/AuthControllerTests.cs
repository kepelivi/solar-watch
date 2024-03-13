using System.Net.Http.Json;
using SolarWatchAPI.Contracts;

namespace SolarWatchIntegrationTest;

[Collection("Integration")]
public class AuthControllerTests
{
    private readonly SolarWatchWebApplicationFactory _application = new();
    private readonly HttpClient _client;

    public AuthControllerTests()
    {
        _client = _application.CreateClient();
    }

    [Fact]
    public async Task Registration_AddsNewUser()
    {
        RegistrationRequest registrationRequest = new("test@gmail.com", "test", "Test12345");

        var response = await _client.PostAsJsonAsync("/Auth/Register", registrationRequest);

        response.EnsureSuccessStatusCode();

        var registrationResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        Assert.Equal("test", registrationResponse.UserName);
        Assert.Equal("test@gmail.com", registrationResponse.Email);
    }

    [Fact]
    public async Task LoginEndpoint_LoginsUserSuccessfully()
    {
        RegistrationRequest registrationRequest = new("test@gmail.com", "test", "Test12345");
        var response = await _client.PostAsJsonAsync("/Auth/Register", registrationRequest);
        response.EnsureSuccessStatusCode();

        AuthReq loginReq = new("test@gmail.com", "Test12345");
        var login = await _client.PostAsJsonAsync("/Auth/Login", loginReq);
        login.EnsureSuccessStatusCode();
        var loginRes = await login.Content.ReadFromJsonAsync<AuthRes>();
        
        Assert.NotNull(loginRes.Token);
        Assert.Equal(loginRes.Email, loginReq.Email);
    }
}