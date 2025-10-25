using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtService
{
    public static string GenerateAccessToken(string username, IConfiguration config)
    {
        var jwt = config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwt["Key"]!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: [new Claim(ClaimTypes.Name, username)],
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["AccessTokenLifetimeMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static RefreshToken GenerateRefreshToken(string username, IConfiguration config)
    {
        var lifetimeDays = double.Parse(config["Jwt:RefreshTokenLifetimeDays"]!);
        return new RefreshToken
        {
            Username = username,
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiresAt = DateTime.UtcNow.AddDays(lifetimeDays),
            IsRevoked = false
        };
    }
}

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
}
