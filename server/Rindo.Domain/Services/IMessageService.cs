namespace Application.Interfaces.Services;

public interface IMessageService
{
    Task<Tuple<string, string>> AddMessage(Guid userId, Guid chatId, string content);
}