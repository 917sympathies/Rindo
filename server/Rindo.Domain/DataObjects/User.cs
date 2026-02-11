using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rindo.Domain.DataObjects;

[Table("users", Schema = "dbo")]
public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<Project> Projects { get; set; }
    public IEnumerable<Invitation> Invitations { get; set; }
    public override bool Equals(object? obj)
    {
        return obj is User user && user.UserId.Equals(UserId);
    }
}