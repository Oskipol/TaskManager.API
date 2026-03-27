using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManager.API.Models;
public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration cg)
    {
        _db=db;
        _config=cg;
    }
    public async Task<User?> Register(RegisterDto dto)
    {
        if(_db.Users.Any(u=>u.Email==dto.Email)) return null;
        var user=new User{Username=dto.Username, Email=dto.Email, HashedPassword=BCrypt.Net.BCrypt.HashPassword(dto.Password)};
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
    public User? Login(LoginDto dto)
    {
        var user=_db.Users.FirstOrDefault(u=>u.Email==dto.Email);
        if(user==null) return null;
        if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword)) return null;
        return user;
    }
    public string GenerateToken(User user)
    {
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var claims=new[]{new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Email, user.Email)};
        var token=new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}