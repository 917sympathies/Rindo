namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity, Guid entityId) : base($"Entity {entity} with the key ({entityId}) was not found") { }
}