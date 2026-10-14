public sealed class AdminUpdateContext<TEntity>
{
    public required TEntity Entity { get; init; }

    public required IServiceProvider Services { get; init; }

    public required HttpContext HttpContext { get; init; }

    public required IReadOnlyDictionary<string, object?> Values { get; init; }

    public CancellationToken CancellationToken =>
        HttpContext.RequestAborted;

    public TService GetService<TService>()
        where TService : notnull
    {
        return Services.GetRequiredService<TService>();
    }
}