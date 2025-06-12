using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ChatRepository : RepositoryBase<Chat>, IChatRepository
{
    public ChatRepository(PostgresDbContext context) : base(context)
    {
    }

    public async Task Create(Chat chat) => await CreateAsync(chat);

    public new void Delete(Chat chat) => base.Delete(chat);

    public async Task<Chat?> GetChatById(Guid id) =>
        await FindByCondition(c => c.Id == id)
            .Include(c => c.Messages)
            .AsNoTracking()
            .FirstOrDefaultAsync();
}