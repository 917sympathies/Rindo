using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Infrastructure.Models;

namespace Rindo.Infrastructure.Services;

public class InvitationService : IInvitationService
{
    private readonly RindoDbContext _context;

    public InvitationService(RindoDbContext context)
    {
        _context = context;
    }

    public async Task<Result> DeleteInvitation(Guid id)
    {
        var invite = await _context.Invitations.FirstOrDefaultAsync(inv => inv.Id == id);
        if (invite == null) return Error.NotFound("Такого приглашения не существует!");
        _context.Invitations.Remove(invite);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}