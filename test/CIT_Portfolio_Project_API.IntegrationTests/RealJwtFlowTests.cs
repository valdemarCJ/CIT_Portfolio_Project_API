using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace CIT_Portfolio_Project_API.IntegrationTests;

public class RealJwtFlowTests : IClassFixture<RealJwtWebApplicationFactory>
{
    private readonly RealJwtWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RealJwtFlowTests(RealJwtWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    private record RegisterResponse(int Id, string Username, string Email);
    private record LoginResponse(string Token, DateTime ExpiresAt);

    [Fact]
    public async Task Login_Then_Call_Protected_Endpoints_With_Bearer_Should_Succeed()
    {
        // Arrange: unique user
        var uname = $"it_{Guid.NewGuid():N}";
        var email = $"{uname}@example.com";
        var pwd = "P@ssw0rd!";
        int userId = 0;

        // Register
        var regResp = await _client.PostAsJsonAsync("/api/users", new { Username = uname, Email = email, Password = pwd });
        regResp.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.Conflict, HttpStatusCode.BadRequest, HttpStatusCode.OK);
        // Try to extract user id when created
        if (regResp.StatusCode == HttpStatusCode.Created || regResp.StatusCode == HttpStatusCode.OK)
        {
            var reg = await regResp.Content.ReadFromJsonAsync<RegisterResponse>();
            if (reg != null) userId = reg.Id;
        }

        // Login
        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", new { Username = uname, Password = pwd });
        loginResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var login = await loginResp.Content.ReadFromJsonAsync<LoginResponse>();
        login.Should().NotBeNull();
        login!.Token.Should().NotBeNullOrWhiteSpace();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);

        // Rate a known movie (protected)
        var rateResp = await _client.PostAsJsonAsync("/api/ratings", new { Tconst = "tt0468569", Value = 9 });
        rateResp.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);

        // If we have user id, exercise bookmarks with matching route id
        if (userId > 0)
        {
            var bmAdd = await _client.PostAsJsonAsync($"/api/users/{userId}/bookmarks", new { Tconst = "tt0468569", Note = "test" });
            bmAdd.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);

            var bmList = await _client.GetAsync($"/api/users/{userId}/bookmarks");
            bmList.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
