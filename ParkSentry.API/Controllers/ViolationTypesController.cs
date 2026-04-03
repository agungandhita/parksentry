using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Infrastructure.Data;

namespace ParkSentry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViolationTypesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ViolationTypesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var types = await _db.ViolationTypes
            .Select(v => new { v.Id, v.Code, v.Name, v.Description, v.FineAmount, v.CreatedAt })
            .OrderBy(v => v.Code)
            .ToListAsync();
        return Ok(types);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vtype = await _db.ViolationTypes.FindAsync(id);
        if (vtype == null) return NotFound();
        return Ok(new { vtype.Id, vtype.Code, vtype.Name, vtype.Description, vtype.FineAmount, vtype.CreatedAt });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateViolationTypeRequest req)
    {
        var vtype = ViolationType.Create(req.Code, req.Name, req.FineAmount, req.Description);
        _db.ViolationTypes.Add(vtype);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = vtype.Id }, new { vtype.Id, vtype.Code, vtype.Name, vtype.FineAmount });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vtype = await _db.ViolationTypes.FindAsync(id);
        if (vtype == null) return NotFound();
        _db.ViolationTypes.Remove(vtype);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public record CreateViolationTypeRequest(string Code, string Name, decimal FineAmount, string? Description);
