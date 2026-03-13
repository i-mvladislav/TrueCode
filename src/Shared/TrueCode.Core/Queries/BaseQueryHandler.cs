namespace TrueCode.Core.Queries;

public abstract class BaseQueryHandler<TRequest, TResponse>
{
    public async Task<QueryResult<TResponse>> ExecuteAsync(TRequest query, CancellationToken cancellationToken = default)
    {
        return await ExecuteCoreAsync(query, cancellationToken);
    }
    
    protected abstract Task<QueryResult<TResponse>> ExecuteCoreAsync(TRequest query, CancellationToken cancellationToken = default);
}