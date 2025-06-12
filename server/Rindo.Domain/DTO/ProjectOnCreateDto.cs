using System.ComponentModel.DataAnnotations;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.DTO;

public class ProjectOnCreateDto
{
    [Required(ErrorMessage = "Необходимо указать название проекта!")]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public DateOnly StartDate { get; set; } 
    public DateOnly FinishDate { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}