using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
 

namespace CIT_Portfolio_Project_API.IntegrationTests;
public class EndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    

    public EndpointsTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Swagger_Json_Should_Return_200()
    {
        var resp = await _client.GetAsync("/swagger/v1/swagger.json");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var json = await resp.Content.ReadAsStringAsync();
        json.Should().Contain("openapi");
    }

    [Fact]
    public async Task Movies_List_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Movies?page=1&pageSize=5");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task People_List_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/People?page=1&pageSize=5");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Movies_GetById_Should_Not_500()
    {
        // Try a commonly present title (The Dark Knight). If not found, endpoint should return 404, but not 500.
        var resp = await _client.GetAsync("/api/Movies/tt0468569");
        resp.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNull();
    }

    [Fact]
    public async Task Analytics_Similar_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Analytics/similar/tt0468569");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Analytics_PopularActors_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Analytics/popular-actors/tt0468569");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Movies_Search_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Movies/search?query=dark");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Movies_Structured_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Movies/structured?title=dark%20knight&person=Christian%20Bale");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task People_GetById_Should_Not_500()
    {
        var resp = await _client.GetAsync("/api/People/nm0000288"); // Christian Bale (likely present)
        resp.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    var text = await resp.Content.ReadAsStringAsync();
    }

    [Fact]
    public async Task Users_GetById_Should_Not_500()
    {
        var resp = await _client.GetAsync("/api/Users/1");
        resp.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    var text = await resp.Content.ReadAsStringAsync();
    }

    [Fact]
    public async Task Bookmarks_List_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/users/1/Bookmarks");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNull();
    }

    [Fact]
    public async Task Search_String_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Search?userId=1&query=dark");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Search_Structured_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Search/structured?userId=1&title=dark%20knight&person=Christian%20Bale");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Analytics_PersonWords_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Analytics/person-words?name=Christian%20Bale");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Analytics_CoPlayers_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Analytics/co-players?actor=Christian%20Bale");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Analytics_ExactMatch_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Analytics/exact-match?query=batman%20joker");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Analytics_BestMatch_Should_Return_200()
    {
        var resp = await _client.GetAsync("/api/Analytics/best-match?query=batman%20joker");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Debug_DbPing_Should_Return_Useful_Status()
    {
        var resp = await _client.GetAsync("/debug/db-ping");
        // In CI without DB it might be 5xx; locally with DB it should be 200
        resp.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.BadRequest);
    var text = await resp.Content.ReadAsStringAsync();
        text.Should().NotBeNull();
    }
}
