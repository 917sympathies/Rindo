namespace Application.Common.Exceptions;

public class NotFoundException(string entity, Guid entityId) : Exception($"Entity {entity} with the key ({entityId}) was not found");