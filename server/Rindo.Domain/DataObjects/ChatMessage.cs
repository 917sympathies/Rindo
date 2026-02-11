using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.DataObjects;

[Table("chat_messages", Schema = "dbo")]
public class ChatMessage
{
    [Key]
    public Guid MessageId { get; set;}
    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
    public string Content { get; set; }
    public DateTime Time { get; set; }
}