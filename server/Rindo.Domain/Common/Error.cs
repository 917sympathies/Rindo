namespace Rindo.Domain.Common;

public record Error
{
    public int Code { get; init; }
    public string Description { get; init; }
    public ErrorType ErrorType { get; init; }

    private Error(int code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    public static Error None => 
        new(200, string.Empty, ErrorType.None);
    
    public static Error NotFound(string description) =>
        new Error(404, description, ErrorType.NotFound);
    
    public static Error Validation(string description) =>
        new Error(401, description, ErrorType.Validation);
    
    public static Error Failure(string description) => 
        new Error(422, description, ErrorType.Failure);

}