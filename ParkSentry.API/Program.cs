using Microsoft.EntityFrameworkCore;
using ParkSentry.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Database (PostgreSQL) ──────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── CORS (untuk Vue.js frontend) ───────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ── Controllers ────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ──────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "ParkSentry API",
        Version = "v1",
        Description = "Sistem Manajemen Pelanggaran Parkir - ParkSentry"
    });
});

var app = builder.Build();

// ── Middleware Pipeline ──────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkSentry API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowVue");
app.UseAuthorization();
app.MapControllers();

app.Run();
