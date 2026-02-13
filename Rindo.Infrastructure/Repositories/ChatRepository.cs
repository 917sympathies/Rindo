using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;

namespace Rindo.Infrastructure.Repositories;

public class ChatRepository(PostgresDbContext context) : RepositoryBase<Chat>(context), IChatRepository
{
    private readonly PostgresDbContext _context = context;
    public async Task<Chat> Create(Chat chat) => await CreateAsync(chat);

    public async Task DeleteById(Guid chatId)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Chat WHERE Id = {chatId}");
    }

    public async Task<Chat?> GetChatById(Guid id) =>
        await FindByCondition(c => c.ChatId == id)
            .Include(c => c.Messages)
            .AsNoTracking()
            .FirstOrDefaultAsync();
}