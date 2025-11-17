using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using StackExchange.Profiling;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog ----------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var configuration = builder.Configuration;

// ---------- Health Checks ----------
builder.Services.AddHealthChecks();

// ---------- MiniProfiler ----------
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
}).AddEntityFramework();

// ---------- Controllers ----------
builder.Services.AddControllers();

// ---------- EF Core ----------
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// ---------- JWT Authentication ----------
var jwtKey = configuration["Jwt:Key"] ?? "SuperSecretKey123";
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiniProfiler();

app.UseAuthentication();
app.UseAuthorization();

// ---------- Middleware ----------
app.UseMiniProfiler();

// ---------- Controllers ----------
app.MapControllers();

app.MapHealthChecks("/health");

app.MapGet("/hello", () => Results.Ok("System Working with Logs, DB and Auth!"));

app.MapPost("/auth/token", (LoginRequest req, IConfiguration cfg) =>
{
    if (req.UserName == "admin" && req.Password == "password")
    {
        var jwtKeyLocal = cfg["Jwt:Key"] ?? "SuperSecretKey123";
        var keyLocal = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyLocal));
        var creds = new SigningCredentials(keyLocal, SecurityAlgorithms.HmacSha256);

        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(cfg["Jwt:ExpireMinutes"] ?? "60")),
            signingCredentials: creds
        );

        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = jwt });
    }
    return Results.Unauthorized();
});

app.Run();

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}

public record LoginRequest(string UserName, string Password);
