using ArtMedica.WebApi.Configuration;

public sealed class AdminBuilder
{
    private readonly AdminRegistry _registry;

    public AdminBuilder(
        AdminRegistry registry)
    {
        _registry = registry;
    }

    public AdminEntityBuilder<TEntity> Entity<TEntity>(
        Action<AdminEntityBuilder<TEntity>> configure)
        where TEntity : class, new()
    {
        var definition =
            new EntityDefinition<TEntity>();

        var builder =
            new AdminEntityBuilder<TEntity>(
                definition);

        configure(builder);

        _registry.Register(definition);

        return builder;
    }
}