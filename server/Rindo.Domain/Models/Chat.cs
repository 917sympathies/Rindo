using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.Models;

[Table("Chats", Schema = "dbo")]
public class Chat
{
    public Guid Id { get; set;}
    public IEnumerable<ChatMessage> Messages { get; set; }
}