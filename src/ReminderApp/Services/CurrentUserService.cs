using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ReminderApp.Data;
using ReminderApp.Models;

namespace ReminderApp.Services;

/// <summary>Maps the signed-in OAuth identity to an <see cref="AppUser"/>, creating it on first sign-in.</summary>
public class CurrentUserService(AuthenticationStateProvider auth, IDbContextFactory<AppDbContext> factory)
{
    public async Task<AppUser?> GetAsync()
    {
        var principal = (await auth.GetAuthenticationStateAsync()).User;
        if (principal.Identity?.IsAuthenticated != true) return null;

        var externalId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(externalId)) return null;

        await using var db = await factory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);
        if (user is null)
        {
            user = new AppUser { ExternalId = externalId };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
        return user;
    }
}
