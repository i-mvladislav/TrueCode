using TrueCode.Core.Models;

namespace TrueCode.Core.Commands;

public class CommandResult
{
    public bool IsSuccess => Errors.Count == 0;
    public IReadOnlyList<Error> Errors { get; init; } = [];

    internal CommandResult()
    {
        
    }
    
    private CommandResult(IEnumerable<Error> errors)
    {
        Errors = errors.ToList();
    }

    public static CommandResult Success()
    {
        return new CommandResult([]);
    }
    
    public static CommandResult Failure(IEnumerable<Error> errors)
    {
        return new CommandResult(errors);
    }
}

public class CommandResult<T> : CommandResult
{
    public T? Data { get; }
    
    private CommandResult(T? data)
    {
        Data = data;
    }

    private CommandResult(IEnumerable<Error> errors)
    {
        Errors = errors.ToList();
    }
    
    public static CommandResult<T> Success(T data)
    {
        return new CommandResult<T>(data);
    }
    
    public static CommandResult<T> Failure(IEnumerable<Error> errors)
    {
        return new CommandResult<T>(errors);
    }
}