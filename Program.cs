using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimesheetAPI.Data;
using System.Text;
using TimesheetAPI.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add JWT settings
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    ["Jwt:Key"] = "ThisIsASecretKeyForJWTTokenGeneration123!",
    ["Jwt:Issuer"] = "https://yourdomain.com",
    ["Jwt:Audience"] = "https://yourdomain.com"
});
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                              // Add DB context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add services
builder.Services.AddAuthorization();
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy)); // 🔐 Applies globally
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Timesheet API", Version = "v1" });

    // 🔐 Add JWT Auth to Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer <your-token>"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapPost("/api/auth/login", (UserLogin user, IConfiguration config) =>
{
    // TODO: Replace with your user validation logic
    if (user.Username == "admin" && user.Password == "password")
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return Results.Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    return Results.Unauthorized();
});
//app.MapGet("/api/timesheets", [Microsoft.AspNetCore.Authorization.Authorize] (DateTime? fromDate, DateTime? toDate, string? userId, ApplicationDbContext db) =>
//{
//    var query = db.TimesheetEntries.AsQueryable();

//    if (fromDate.HasValue)
//    {
//        query = query.Where(t => t.Date >= fromDate.Value);
//    }

//    if (toDate.HasValue)
//    {
//        query = query.Where(t => t.Date <= toDate.Value);
//    }

//    if (!string.IsNullOrEmpty(userId))
//    {
//        query = query.Where(t => t.Id == Convert.ToInt32(userId));
//    }

//    return query.ToList();
//});

app.Run();
