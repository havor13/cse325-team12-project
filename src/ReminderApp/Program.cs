using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ReminderApp.Components;
using ReminderApp.Data;
using ReminderApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

// Database. SQLite for local development; switch the provider (PostgreSQL / SQL Server)
// and the connection string when deploying to the cloud.
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Authentication. Cookie-only for now: sign-in is a development stub (see /login below).
// TODO (feature 1): replace the stub with real OAuth (Google, Microsoft, or GitHub).
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();

builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();

// Dev convenience: create the database on startup. Replace with EF migrations before deployment:
//   dotnet ef migrations add InitialCreate
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = factory.CreateDbContext();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// DEVELOPMENT ONLY: signs in as a test user so the team can work without OAuth credentials.
// Use /login?user=alice to switch between test users (each has separate tasks and points).
app.MapGet("/login", async (HttpContext ctx, string? user, string? returnUrl) =>
{
    if (!app.Environment.IsDevelopment()) return Results.NotFound();

    var name = string.IsNullOrWhiteSpace(user) ? "dev-user" : user.Trim().ToLowerInvariant();
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, $"dev:{name}"),
        new Claim(ClaimTypes.Name, name),
        new Claim("quote", QuoteService.GetRandom()), // feature 8: new quote at each sign-in
    };
    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
    await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    var target = !string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith('/') && !returnUrl.StartsWith("//")
        ? returnUrl : "/dashboard";
    return Results.Redirect(target);
});

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
