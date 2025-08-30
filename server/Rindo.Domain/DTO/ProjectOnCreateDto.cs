using System.ComponentModel.DataAnnotations;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.DTO;

public class ProjectOnCreateDto
{
    [Required]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public DateOnly StartDate { get; set; } 
    public Tag[] Tags { get; set; }
}