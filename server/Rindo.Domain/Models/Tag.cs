using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.Models;

[Table("Tags", Schema = "dbo")]
public class Tag
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid ProjectId { get; init; }
}