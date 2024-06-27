namespace Rindo.Domain.Common;

public class Result : IResult
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Failure<T>(Error error) => new(default!, false, error);

    public static implicit operator Result(Error error) => Failure(error);

}

public sealed class Result<T> : Result, IResult<T>
{
    public T Value { get; }
    internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error) => Value = value;

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure<T>(error);

}