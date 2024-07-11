namespace Rindo.Domain.Entities;

public class Chat
{
    public Guid Id { get; set;}
    public IEnumerable<ChatMessage> Messages { get; set; } = default!;
}