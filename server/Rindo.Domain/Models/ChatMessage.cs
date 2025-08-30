using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.Models;

[Table("ChatMessages", Schema = "dbo")]
public class ChatMessage
{
    public Guid Id { get; set;}
    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
    public string Content { get; set; }
    public DateTime Time { get; set; }
}