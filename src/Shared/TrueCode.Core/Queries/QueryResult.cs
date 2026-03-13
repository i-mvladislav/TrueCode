using TrueCode.Core.Models;

namespace TrueCode.Core.Queries;

public sealed class QueryResult<T>
{
    public bool IsSuccess => Errors.Count == 0;
    public T? Data { get; }
    public IReadOnlyList<Error> Errors { get; } = [];

    private QueryResult(T? data)
    {
        Data = data;
    }

    private QueryResult(IEnumerable<Error> errors)
    {
        Errors = errors.ToList();
    }
    
    public static QueryResult<T> Success(T data)
    {
        return new QueryResult<T>(data);
    }
    
    public static QueryResult<T> Failure(IEnumerable<Error> errors)
    {
        return new QueryResult<T>(errors);
    }
}