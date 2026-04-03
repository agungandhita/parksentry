using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Infrastructure.Data;

namespace ParkSentry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingZonesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ParkingZonesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var zones = await _db.ParkingZones
            .Where(z => z.IsActive)
            .Select(z => new { z.Id, z.ZoneCode, z.ZoneName, z.Address, z.City, z.IsActive, z.CreatedAt })
            .OrderBy(z => z.ZoneCode)
            .ToListAsync();
        return Ok(zones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var zone = await _db.ParkingZones.FindAsync(id);
        if (zone == null) return NotFound();
        return Ok(new { zone.Id, zone.ZoneCode, zone.ZoneName, zone.Address, zone.City, zone.IsActive, zone.CreatedAt });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateZoneRequest req)
    {
        var zone = ParkingZone.Create(req.ZoneCode, req.ZoneName, req.Address, req.City);
        _db.ParkingZones.Add(zone);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = zone.Id }, new { zone.Id, zone.ZoneCode, zone.ZoneName });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var zone = await _db.ParkingZones.FindAsync(id);
        if (zone == null) return NotFound();
        zone.Deactivate();
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public record CreateZoneRequest(string ZoneCode, string ZoneName, string? Address, string? City);
