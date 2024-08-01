namespace Rindo.Domain.Common;

public record Error
{
    public string Description { get; init; }
    public ErrorType ErrorType { get; init; }

    private Error(string description, ErrorType errorType)
    {
        Description = description;
        ErrorType = errorType;
    }

    public static Error None => 
        new(string.Empty, ErrorType.None);
    
    public static Error NotFound(string description) =>
        new(description, ErrorType.NotFound);
    
    public static Error Validation(string description) =>
        new(description, ErrorType.Validation);
    
    public static Error Failure(string description) => 
        new(description, ErrorType.Failure);

}