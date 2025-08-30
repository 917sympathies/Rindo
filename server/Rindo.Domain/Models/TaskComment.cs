using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.Models;

[Table("TaskComments", Schema = "dbo")]
public class TaskComment
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTime Time { get; set; }
}