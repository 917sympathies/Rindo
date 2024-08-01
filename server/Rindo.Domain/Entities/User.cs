using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Rindo.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public ICollection<Project> Projects { get; set; } = default!;
    public ICollection<Invitation> Invitations { get; set; } = default!;
    public override bool Equals(object? obj)
    {
        return obj is User user && user.Id.Equals(this.Id);
    }
}