public static class AuthEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/login", (UserLoginDto login, IConfiguration config, HttpResponse response) =>
        {
            if (login.Username != "admin" || login.Password != "password")
                return Results.Unauthorized();

            var accessToken = JwtService.GenerateAccessToken(login.Username, config);
            var refreshToken = JwtService.GenerateRefreshToken(login.Username, config);

            TokenStore.RefreshTokens.Add(refreshToken);

            response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = refreshToken.ExpiresAt
            });

            return Results.Ok(new
            {
                accessToken,
                refreshToken = refreshToken.Token
            });
        });

        group.MapPost("/refresh", (HttpRequest req, HttpResponse res, IConfiguration config) =>
        {
            if (!req.Cookies.TryGetValue("refreshToken", out var token))
                return Results.Unauthorized();

            var stored = TokenStore.RefreshTokens
                .SingleOrDefault(t => t.Token == token);

            if (stored is null)
                return Results.Ok();

            if (stored.IsRevoked || stored.ExpiresAt < DateTime.UtcNow)
                return Results.Unauthorized();

            stored.IsRevoked = true;

            var newAccessToken = JwtService.GenerateAccessToken(stored.Username, config);
            var newRefreshToken = JwtService.GenerateRefreshToken(stored.Username, config);

            TokenStore.RefreshTokens.Add(newRefreshToken);

            res.Cookies.Append("refreshToken", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = newRefreshToken.ExpiresAt
            });

            return Results.Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        });

        group.MapPost("/logout", (HttpResponse res, HttpRequest req) =>
        {
            if (req.Cookies.TryGetValue("refreshToken", out var token))
            {
                var stored = TokenStore.RefreshTokens.SingleOrDefault(t => t.Token == token);
                if (stored is not null)
                    stored.IsRevoked = true;
            }

            res.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Results.Ok();
        });
    }
}


public record UserLoginDto(string Username, string Password);

public record TokenRefreshDto(string RefreshToken);