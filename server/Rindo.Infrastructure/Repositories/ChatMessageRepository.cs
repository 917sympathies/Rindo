using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
{
    public ChatMessageRepository(RindoDbContext context) : base(context)
    {
    }

    public Task AddMessage(ChatMessage message) => CreateAsync(message);

    public async Task<IEnumerable<ChatMessage>> GetMessagesFromChat(Guid chatId) =>
        await FindByCondition(c => c.ChatId == chatId).ToListAsync();
}