using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Rindo.Infrastructure.Models;

namespace Rindo.API.Filters;

public class AsyncActionAccessFilter : IAsyncActionFilter
{
    private readonly RindoDbContext _context;

    public AsyncActionAccessFilter(RindoDbContext context)
    {
        _context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext?.Request.Cookies["_rindo"];
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var userId = jwtSecurityToken.Claims.First(c => c.Type == "userId").Value;
        if (context.ActionArguments.TryGetValue("id", out var id))
        {
            var project = await _context.Projects
                .Include(project => project.Users)
                .Include(project => project.Owner)
                .FirstOrDefaultAsync(p => p.Id == (Guid)id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
            if (project.Users.Contains(user) || project.Owner.Id.Equals(user.Id)) await next();
        }
        if (context.ActionArguments.TryGetValue("projectId", out var projectId))
        {
            var project = await _context.Projects
                .Include(project => project.Users)
                .Include(project => project.Owner)
                .FirstOrDefaultAsync(p => p.Id == (Guid)projectId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
            if (project.Users.Contains(user) || project.Owner.Id.Equals(user.Id)) await next();
        }
        context.Result = new BadRequestResult();
    }
}