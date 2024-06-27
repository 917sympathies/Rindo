namespace Rindo.Domain.DTO;

public class RoleDtoOnCreate
{
    public string Name { get; set; } = default!;
    public Guid ProjectId { get; set; }
    public string Color { get; set; } = default!;
}