using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Infrastructure.Data;

namespace ParkSentry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db) => _db = db;

    // ── POST /api/auth/login ─────────────────────────────────────
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new { message = "Username dan password wajib diisi" });

        var foundUser = await _db.Users
            .Include(u => u.Role)
            .Include(u => u.Officer)
            .Include(u => u.VehicleOwner)
            .FirstOrDefaultAsync(u => u.Username == req.Username);

        if (foundUser == null || foundUser.Password != req.Password)
            return Unauthorized(new { message = "Username atau password salah" });

        if (!foundUser.IsActive)
            return Unauthorized(new { message = "Akun tidak aktif. Hubungi administrator." });

        var roleName = foundUser.Role.Name.ToLower(); // "admin", "officer", "owner"

        return Ok(new
        {
            id        = foundUser.Id,
            username  = foundUser.Username,
            fullName  = foundUser.FullName,
            email     = foundUser.Email,
            phone     = foundUser.Phone,
            role      = roleName,
            roleId    = foundUser.RoleId,
            officerId = foundUser.Officer?.Id,
            ownerId   = foundUser.VehicleOwner?.Id,
        });
    }

    // ── POST /api/auth/register ──────────────────────────────────
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(req.Username) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            string.IsNullOrWhiteSpace(req.FullName))
        {
            return BadRequest(new { message = "Nama lengkap, username, dan password wajib diisi" });
        }

        if (req.Password.Length < 6)
            return BadRequest(new { message = "Password minimal 6 karakter" });

        // Check username uniqueness
        var exists = await _db.Users.AnyAsync(u => u.Username == req.Username);
        if (exists)
            return Conflict(new { message = "Username sudah digunakan" });

        // Resolve role
        var roleName = (req.Role ?? "owner").ToLower();
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
            return BadRequest(new { message = $"Role '{roleName}' tidak ditemukan" });

        // Create user
        var newUser = ParkSentry.Domain.Entities.User.Create(role.Id, req.FullName, req.Username, req.Password, req.Email, req.Phone);
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        // If role is "owner", also create a vehicle_owner record
        if (roleName == "owner")
        {
            var idCardNo = req.IdCardNo ?? $"AUTO-{newUser.Id:D6}";
            var owner = VehicleOwner.Create(newUser.Id, idCardNo, req.Address);
            _db.VehicleOwners.Add(owner);
            await _db.SaveChangesAsync();
        }

        return Ok(new
        {
            message  = "Registrasi berhasil",
            id       = newUser.Id,
            username = newUser.Username,
            fullName = newUser.FullName,
            role     = roleName,
        });
    }
}

public record LoginRequest(string Username, string Password);

public record RegisterRequest(
    string FullName,
    string Username,
    string Password,
    string? Email,
    string? Phone,
    string? Role,        // "admin" | "officer" | "owner"
    string? IdCardNo,    // required only if role == "owner"
    string? Address
);
