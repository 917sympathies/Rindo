using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.DataObjects;

[Table("chats", Schema = "dbo")]
public class Chat
{
    [Key]
    public Guid ChatId { get; set;}
    public IEnumerable<ChatMessage> Messages { get; set; }
}