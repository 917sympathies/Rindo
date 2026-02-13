using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rindo.Domain.Enums;

namespace Rindo.Domain.DataObjects;

[Table("roles", Schema = "dbo")]
public class Role
{
    [Key]
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public ICollection<User> Users { get; set; }
    public Guid ProjectId { get; set; }
    public Permissions BitPermissions { get; set; }
}