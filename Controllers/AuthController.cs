using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Persistence;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

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

        return Ok("Kullanıcı başarıyla oluşturuldu.");
    }
}