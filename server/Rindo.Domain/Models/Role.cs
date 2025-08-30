using System.ComponentModel.DataAnnotations.Schema;
using Rindo.Domain.Enums;

namespace Rindo.Domain.Models;

[Table("Roles", Schema = "dbo")]
public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public ICollection<User> Users { get; set; }
    public Guid ProjectId { get; set; }
    public RoleRights BitRoleRights { get; set; }
}