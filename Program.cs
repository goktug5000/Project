using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
// Ensure authentication and authorization middleware is properly configured.
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
{
    if (!claims.Identity?.IsAuthenticated ?? true)
    {
        return Results.Unauthorized();
    }

    string userId = claims.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var user = await context.Users.FindAsync(userId);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.MapGet("users/getAll", async (ApplicationDbContext context) =>
{
    var users = context.Users.Select(x => new { x.Id, x.UserName }).ToList();
    return Results.Ok(users);
});

app.MapGet("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

    return Results.Ok(new { Message = "Logout successful." });
});

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();
