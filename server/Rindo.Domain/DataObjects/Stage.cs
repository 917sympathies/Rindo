using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.Enums;

namespace Rindo.Domain.DataObjects;

[Table("stages", Schema = "dbo")]
public class Stage
{
    [Key]
    public Guid StageId { get; set; }
    public string? CustomName { get; set; }
    public Guid ProjectId { get; set; }
    public IEnumerable<ProjectTask> Tasks { get; set; }
    public int Index { get; set; }
    public StageType Type { get; set; }
}

public class StageDto
{
    public Guid Id { get; set; }
    public string? CustomName { get; set; }
    public Guid ProjectId { get; set; }
    public IEnumerable<ProjectTaskDto> Tasks { get; set; }
    public int Index { get; set; }
    public StageType Type { get; set; }
}