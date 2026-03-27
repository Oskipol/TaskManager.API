public record RegisterDto(string Username, string Email, string Password);
public record LoginDto(string Email, string Password);
public record AuthTokenDto(string Token, string Username);