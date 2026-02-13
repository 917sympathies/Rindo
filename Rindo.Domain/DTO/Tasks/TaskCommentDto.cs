namespace Rindo.Domain.DTO.Tasks;

public class TaskCommentDto
{
    public UserShortInfoDto User { get; set; }
    public string Content { get; set; }
    public DateTimeOffset Time { get; set; }
}