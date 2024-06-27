using System.Runtime.Serialization;
using Rindo.Domain.DTO;

namespace Rindo.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set;}
    public Guid SenderId { get; set; }
    [IgnoreDataMember]
    public Chat Chat { get; set; } = default!;
    public Guid ChatId { get; set; }
    public string Username { get; set; }
    public string Content { get; set; }
    // Добавить content и убрать Sender и Chat
    // Добавить DateTime дату отправления и, соответственно, при взятии данных из БД сортировать их в определенном порядке
}