using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using CIT_Portfolio_Project_API.Models.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace CIT_Portfolio_Project_API.IntegrationTests;

public class JwtSmokeTests
{
    [Fact]
    public void Generate_And_Validate_Token_Works_Without_Db()
    {
        // Arrange: in-memory JWT config
        var key = "THIS_IS_A_TEST_KEY_AT_LEAST_32_CHARS_LONG_123"; // 48 chars
        var inMemorySettings = new Dictionary<string, string?>
        {
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:Key"] = key,
            ["Jwt:ExpiresMinutes"] = "5"
        };
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var svc = new JwtTokenService(config);
        var user = new User { Id = 42, Username = "alice", Email = "alice@example.com", PasswordHash = new string('x', 20) };

        // Act: generate token
        var (token, expiresAt) = svc.GenerateToken(user);

        // Assert basic shape
        token.Should().NotBeNullOrWhiteSpace();
        expiresAt.Should().BeAfter(DateTime.UtcNow);

        // Validate token using same parameters as Program.cs
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        tokenHandler.ValidateToken(token, validationParams, out var _).Identity.Should().NotBeNull();

        var principal = tokenHandler.ValidateToken(token, validationParams, out var _);
        var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                 ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var uname = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
                    ?? principal.FindFirst(ClaimTypes.Name)?.Value;

        sub.Should().Be("42");
        uname.Should().Be("alice");
    }
}
