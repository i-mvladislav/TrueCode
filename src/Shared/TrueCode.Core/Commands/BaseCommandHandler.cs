namespace TrueCode.Core.Commands;

public abstract class BaseCommandHandler<TRequest>
{
    public async Task<CommandResult> ExecuteAsync(TRequest command, CancellationToken cancellationToken = default)
    {
        return await ExecuteCoreAsync(command, cancellationToken);
    }
    
    protected abstract Task<CommandResult> ExecuteCoreAsync(TRequest command, CancellationToken cancellationToken = default);
}

public abstract class BaseCommandHandler<TRequest, TResponse>
{
    public async Task<CommandResult<TResponse>> ExecuteAsync(TRequest command, CancellationToken cancellationToken = default)
    {
        return await ExecuteCoreAsync(command, cancellationToken);
    }
    
    protected abstract Task<CommandResult<TResponse>> ExecuteCoreAsync(TRequest command, CancellationToken cancellationToken = default);
}