namespace Rindo.Domain.Models;

public class Chat
{
    public Guid Id { get; set;}
    public IEnumerable<ChatMessage> Messages { get; set; }
}