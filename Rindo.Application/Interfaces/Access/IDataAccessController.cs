namespace Application.Interfaces.Access;

public interface IDataAccessController
{
    public Guid EmployeeId { get; set; }
    public Guid[] AccessibleProjectsIds { get; set; }
}