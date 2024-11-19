using System.Runtime.Serialization;

namespace Rindo.Domain.Models;

public class ChatMessage
{
    public Guid Id { get; set;}
    public Guid SenderId { get; set; }
    [IgnoreDataMember]
    public Chat Chat { get; set; } = default!;
    public Guid ChatId { get; set; }
    public string Content { get; set; }
    public DateTime Time { get; set; } = default!;
}