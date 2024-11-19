namespace Rindo.Domain.DTO;

public class MessageDto
{
    public Guid Id { get; set; }
    
    public Guid ChatId { get; set; }
    
    public string Content { get; set; }
    
    public string Username { get; set; }
    
    public DateTime Time { get; set; }
}