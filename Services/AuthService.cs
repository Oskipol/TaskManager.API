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
    public async Task<User> Register(RegisterDto dto)
    {
        if(_db.Users.Any(u=>u.Email==dto.Email)) return null!;
        var user=new User{Username=dto.Username, Email=dto.Email, HashedPassword=BCrypt.Net.BCrypt.HashPassword(dto.Password)};
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
    public User? Login 
}