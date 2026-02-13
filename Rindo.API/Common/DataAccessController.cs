using Application.Interfaces.Access;

namespace Rindo.API.Common;

public class DataAccessController: IDataAccessController
{
    public Guid EmployeeId { get; set; }
    public Guid[] AccessibleProjectsIds { get; set; }
}