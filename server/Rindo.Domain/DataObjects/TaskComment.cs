using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.DataObjects;

[Table("task_comments", Schema = "dbo")]
public class TaskComment
{
    [Key]
    public Guid CommentId { get; set; }
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTimeOffset Time { get; set; }
}