using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Domain.Enums;
using ParkSentry.Infrastructure.Data;

namespace ParkSentry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViolationTicketsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ViolationTicketsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status = null)
    {
        var query = _db.ViolationTickets
            .Include(t => t.Vehicle)
            .Include(t => t.Officer).ThenInclude(o => o.User)
            .Include(t => t.ViolationType)
            .Include(t => t.Zone)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<TicketStatus>(status, true, out var ticketStatus))
            query = query.Where(t => t.Status == ticketStatus);

        var tickets = await query
            .OrderByDescending(t => t.ViolationDate)
            .Select(t => new
            {
                t.Id,
                t.TicketNo,
                t.ViolationDate,
                t.FineAmount,
                t.DueDate,
                t.LocationDetail,
                Status = t.Status.ToString(),
                Vehicle = new { t.Vehicle.PlateNumber, t.Vehicle.Brand, t.Vehicle.Model, t.Vehicle.Color },
                Officer = new { t.Officer.BadgeNo, t.Officer.User.FullName },
                ViolationType = new { t.ViolationType.Code, t.ViolationType.Name },
                Zone = new { t.Zone.ZoneCode, t.Zone.ZoneName }
            })
            .ToListAsync();

        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _db.ViolationTickets
            .Include(t => t.Vehicle).ThenInclude(v => v.Owner).ThenInclude(o => o.User)
            .Include(t => t.Officer).ThenInclude(o => o.User)
            .Include(t => t.ViolationType)
            .Include(t => t.Zone)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return NotFound();

        return Ok(new
        {
            ticket.Id,
            ticket.TicketNo,
            ticket.ViolationDate,
            ticket.FineAmount,
            ticket.DueDate,
            ticket.LocationDetail,
            ticket.PhotoEvidence,
            ticket.Notes,
            Status = ticket.Status.ToString(),
            Vehicle = new { ticket.Vehicle.PlateNumber, ticket.Vehicle.Brand, ticket.Vehicle.Model, ticket.Vehicle.Color },
            Owner = new { ticket.Vehicle.Owner.User.FullName, ticket.Vehicle.Owner.User.Phone, ticket.Vehicle.Owner.IdCardNo },
            Officer = new { ticket.Officer.BadgeNo, ticket.Officer.User.FullName, ticket.Officer.Zone },
            ViolationType = new { ticket.ViolationType.Code, ticket.ViolationType.Name, ticket.ViolationType.FineAmount },
            Zone = new { ticket.Zone.ZoneCode, ticket.Zone.ZoneName, ticket.Zone.Address, ticket.Zone.City }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest req)
    {
        // Auto-generate ticket number
        var count = await _db.ViolationTickets.CountAsync();
        var ticketNo = $"TKT-{DateTime.Now:yyyyMMdd}-{(count + 1):D4}";

        var ticket = ViolationTicket.Create(
            ticketNo,
            req.OfficerId,
            req.VehicleId,
            req.ViolationTypeId,
            req.ZoneId,
            req.FineAmount,
            req.ViolationDate,
            req.LocationDetail,
            req.PhotoEvidence,
            req.Notes
        );

        _db.ViolationTickets.Add(ticket);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id },
            new { ticket.Id, ticket.TicketNo, Status = ticket.Status.ToString() });
    }

    [HttpPatch("{id}/pay")]
    public async Task<IActionResult> MarkAsPaid(int id)
    {
        var ticket = await _db.ViolationTickets.FindAsync(id);
        if (ticket == null) return NotFound();
        ticket.MarkAsPaid();
        await _db.SaveChangesAsync();
        return Ok(new { message = "Tiket berhasil ditandai sebagai lunas", Status = ticket.Status.ToString() });
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var ticket = await _db.ViolationTickets.FindAsync(id);
        if (ticket == null) return NotFound();
        ticket.Cancel();
        await _db.SaveChangesAsync();
        return Ok(new { message = "Tiket berhasil dibatalkan", Status = ticket.Status.ToString() });
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var total = await _db.ViolationTickets.CountAsync();
        var unpaid = await _db.ViolationTickets.CountAsync(t => t.Status == TicketStatus.Unpaid);
        var paid = await _db.ViolationTickets.CountAsync(t => t.Status == TicketStatus.Paid);
        var totalFine = await _db.ViolationTickets.SumAsync(t => t.FineAmount);
        var paidFine = await _db.ViolationTickets
            .Where(t => t.Status == TicketStatus.Paid)
            .SumAsync(t => t.FineAmount);

        return Ok(new { total, unpaid, paid, totalFine, paidFine });
    }
}

public record CreateTicketRequest(
    int OfficerId,
    int VehicleId,
    int ViolationTypeId,
    int ZoneId,
    decimal FineAmount,
    DateTime ViolationDate,
    string? LocationDetail,
    string? PhotoEvidence,
    string? Notes
);
