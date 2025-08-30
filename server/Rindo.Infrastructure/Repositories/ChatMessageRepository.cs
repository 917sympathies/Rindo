using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class ChatMessageRepository(PostgresDbContext context) : RepositoryBase<ChatMessage>(context), IChatMessageRepository
{
    public Task AddMessage(ChatMessage message) => CreateAsync(message);

    public async Task<IEnumerable<ChatMessage>> GetMessagesFromChat(Guid chatId) =>
        await FindByCondition(c => c.ChatId == chatId).ToListAsync();
}