using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Domain.Enums;
using ParkSentry.Infrastructure.Data;

namespace ParkSentry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly AppDbContext _db;
    public VehiclesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _db.Vehicles
            .Include(v => v.Owner).ThenInclude(o => o.User)
            .Where(v => v.IsActive)
            .Select(v => new
            {
                v.Id,
                v.PlateNumber,
                v.Brand,
                v.Model,
                v.Color,
                VehicleType = v.VehicleType.ToString(),
                v.Year,
                v.IsActive,
                Owner = new { v.Owner.User.FullName, v.Owner.IdCardNo }
            })
            .ToListAsync();
        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehicle = await _db.Vehicles
            .Include(v => v.Owner).ThenInclude(o => o.User)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle == null) return NotFound();
        return Ok(new
        {
            vehicle.Id,
            vehicle.PlateNumber,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Color,
            VehicleType = vehicle.VehicleType.ToString(),
            vehicle.Year,
            vehicle.IsActive,
            Owner = new { vehicle.Owner.User.FullName, vehicle.Owner.IdCardNo }
        });
    }

    [HttpGet("plate/{plateNumber}")]
    public async Task<IActionResult> GetByPlate(string plateNumber)
    {
        var vehicle = await _db.Vehicles
            .Include(v => v.Owner).ThenInclude(o => o.User)
            .FirstOrDefaultAsync(v => v.PlateNumber.ToLower() == plateNumber.ToLower());
        if (vehicle == null) return NotFound(new { message = $"Kendaraan dengan plat {plateNumber} tidak ditemukan" });
        return Ok(new
        {
            vehicle.Id,
            vehicle.PlateNumber,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Color,
            VehicleType = vehicle.VehicleType.ToString(),
            vehicle.Year,
            Owner = new { vehicle.Owner.User.FullName, vehicle.Owner.IdCardNo }
        });
    }
}
