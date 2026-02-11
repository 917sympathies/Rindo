namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, Guid entityId) : base($"Entity {entityName} with the key ({entityId}) was not found")
    {
    }

    public NotFoundException(string message) : base($"${message}")
    {
    }
    
    public NotFoundException()
    {
    }
}

public class NotFoundException<TEntity> : Exception
{
    public NotFoundException(string message) : base($"Entity {nameof(TEntity)}: ${message}")
    {
    }
}