using System.Net.Http.Json;
using SolarWatchAPI.Contracts;

namespace SolarWatchIntegrationTest;

[Collection("Integration")]
public class SolarWatchControllerTests
{
    private readonly SolarWatchWebApplicationFactory _application = new();
    private readonly HttpClient _client;

    public SolarWatchControllerTests()
    {
        _client = _application.CreateClient();
    }
    
    [Fact]
    public async Task SolarController_SuccessfullyGivesBackData()
    {
        RegistrationRequest registrationRequest = new("test@gmail.com", "test", "Test12345");
        var response = await _client.PostAsJsonAsync("/Auth/Register", registrationRequest);
        response.EnsureSuccessStatusCode();

        AuthReq loginReq = new("test@gmail.com", "Test12345");
        var login = await _client.PostAsJsonAsync("/Auth/Login", loginReq);
        login.EnsureSuccessStatusCode();

        var solarRes = await _client.GetAsync("SolarWatch/GetSolarWatch?cityName=Budapest&date=2022-12-13");
        solarRes.EnsureSuccessStatusCode();
    }
}