using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ReminderApp.Components;
using ReminderApp.Data;
using ReminderApp.Services;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

// Database. SQLite for local development; switch the provider (PostgreSQL / SQL Server)
// and the connection string when deploying to the cloud.
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// OAuth authentication. Google is the only provider supported for now.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login")
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
    ?? throw new InvalidOperationException("Missing Authentication:Google:ClientId");
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
            ?? throw new InvalidOperationException("Missing Authentication:Google:ClientSecret");
    });
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

// OAuth challenge endpoints. The login endpoint redirects to Google, and the logout endpoint clears the cookie.
app.MapGet("/login", (string? returnUrl) =>
{
    var target = !string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith('/') && !returnUrl.StartsWith("//")
        ? returnUrl : "/dashboard";

    var props = new AuthenticationProperties { RedirectUri = target };
    return Results.Challenge(props, [GoogleDefaults.AuthenticationScheme]);
});

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
