using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;

namespace Rindo.Infrastructure.Repositories;

public class InvitationRepository(PostgresDbContext context) : IInvitationRepository
{
    public async Task<Invitation> CreateInvitation(Invitation invitation)
    {
        var createdEntity = await context.Invitations.AddAsync(invitation);
        await context.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public Task<Invitation?> GetInvitationById(Guid id)
    {
        return context.Invitations.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Invitation[]> GetInvitationsByProjectId(Guid projectId)
    {
        return await context.Invitations.Where(x => x.ProjectId == projectId).ToArrayAsync();
    }

    public async Task<Invitation[]> GetInvitationsByUserId(Guid userId)
    {
        return await context.Invitations.Where(x => x.RecipientId == userId).ToArrayAsync();
    }

    public async Task DeleteInvitation(Invitation invitation)
    {
        context.Invitations.Remove(invitation);
        await context.SaveChangesAsync();
    }
}