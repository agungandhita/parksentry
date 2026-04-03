using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Infrastructure.Data;

namespace ParkSentry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _db;
    public RolesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _db.Roles
            .Select(r => new { r.Id, r.Name, r.Description, r.CreatedAt })
            .ToListAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await _db.Roles.FindAsync(id);
        if (role == null) return NotFound();
        return Ok(new { role.Id, role.Name, role.Description, role.CreatedAt });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest req)
    {
        var role = Role.Create(req.Name, req.Description);
        _db.Roles.Add(role);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, new { role.Id, role.Name, role.Description });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _db.Roles.FindAsync(id);
        if (role == null) return NotFound();
        _db.Roles.Remove(role);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public record CreateRoleRequest(string Name, string? Description);
