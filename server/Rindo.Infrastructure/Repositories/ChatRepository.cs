using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ChatRepository : RepositoryBase<Chat>, IChatRepository
{
    public ChatRepository(RindoDbContext context) : base(context)
    {
    }

    public async Task Create(Chat chat) => await CreateAsync(chat);

    public async Task Delete(Chat chat) => await DeleteAsync(chat);

    public async Task<Chat?> GetChatById(Guid id) =>
        await FindByCondition(c => c.Id == id)
            .Include(c => c.Messages)
            .AsNoTracking()
            .FirstOrDefaultAsync();
}