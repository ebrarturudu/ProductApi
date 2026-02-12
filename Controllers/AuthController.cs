using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 
        }
        
    [AllowAnonymous] 
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Kullanici basariyla olusturuldu.");
    }

    [AllowAnonymous]
    [HttpPost("login")]
public async Task<IActionResult> Login(LoginDto request)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
    if (user == null) return BadRequest("Kullanici bulunamadi.");

    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
    {
        return BadRequest("Hatali sifre.");
    }

    var token = CreateToken(user);

    return Ok(new { token });
}

private string CreateToken(User user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("Id", user.Id.ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        _configuration.GetSection("Jwt:Key").Value!));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

    var tokenDescriptor = new JwtSecurityToken(
        issuer: _configuration.GetSection("Jwt:Issuer").Value,
        audience: _configuration.GetSection("Jwt:Audience").Value,
        claims: claims,
        expires: DateTime.Now.AddDays(1), 
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
}
}