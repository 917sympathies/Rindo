namespace Rindo.Domain.Common;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure => !IsSuccess;
    Error Error { get; }

}

public interface IResult<T> : IResult
{
    T Value { get; }
}