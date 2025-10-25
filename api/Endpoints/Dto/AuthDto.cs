public record UserLoginDto(string Username, string Password);

public record TokenRefreshDto(string RefreshToken);