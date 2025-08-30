namespace Application.Common.Exceptions;

public class NotFoundException(string entityName, Guid entityId) : Exception($"Entity {entityName} with the key ({entityId}) was not found");
public class NotFoundException<TEntity>(string message): Exception($"Entity {nameof(TEntity)}: ${message}");