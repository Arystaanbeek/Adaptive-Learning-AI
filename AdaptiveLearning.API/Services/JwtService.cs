using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AdaptiveLearning.API.Models;

namespace AdaptiveLearning.API.Services;

public class JwtService
{
    private readonly string _secret;
    private readonly int _expiryDays;

    public JwtService(IConfiguration config)
    {
        _secret     = config["Jwt:Secret"] ?? "AdaptiveLearningSecretKey2024!!";
        _expiryDays = int.Parse(config["Jwt:ExpiryDays"] ?? "7");
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:   "AdaptiveLearning",
            audience: "AdaptiveLearning",
            claims:   claims,
            expires:  DateTime.UtcNow.AddDays(_expiryDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenValidationParameters GetValidationParams()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = "AdaptiveLearning",
            ValidAudience            = "AdaptiveLearning",
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secret))
        };
    }
}
