using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Genres
                           .Select(genre => genre.ToDto())
                           .AsNoTracking()
                           .ToListAsync())
            .RequireAuthorization(policy =>
            {
                // Endpoint will be authorized if JWT contains claim "role":"admin"
                policy.RequireRole("admin");
            });

        group.MapGet("/lookup", async (ClaimsPrincipal user, GameStoreContext dbContext) =>
        {
            ArgumentNullException.ThrowIfNull(user.Identity?.Name);

            var top = int.MaxValue;
            if (user.HasClaim(claim => claim.Type == "top"))
            {
                top = int.Parse(user.FindFirstValue("top") ?? throw new Exception("Claim `top` has no value"));
            }

            var searchWord = user.Identity.Name;

            return await dbContext.Genres
                .Where(genre => genre.Name.Contains(searchWord))
                .Take(top)
                .Select(genre => genre.ToDto())
                .AsNoTracking()
                .ToListAsync();
        })
        .RequireAuthorization(policy =>
        {
            // Endpoint will be authorized if JWT contains claim "role":"player"
            policy.RequireRole("player");
        });

        return group;
    }
}