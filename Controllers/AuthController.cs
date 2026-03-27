using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth)=> _auth=auth;
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user=await _auth.Register(dto);
        if(user==null) return BadRequest("Email already in use");
        var token=_auth.GenerateToken(user);
        return Ok(new AuthTokenDto(token, user.Username));
    }
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user=_auth.Login(dto);
        if(user==null) return BadRequest("Invalid credentials");
        var token=_auth.GenerateToken(user);
        return Ok(new AuthTokenDto(token, user.Username));
    }
}